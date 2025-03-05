using UnityEngine;

public class MoveObjectTool : Tools
{
    private bool isDragging = false;
    private Rigidbody objectiveRb;
    private float maxRadius;
    private IMovable movable;
    private Vector3 selectedAxis; // Nuevo: Almacena el eje seleccionado
    private Vector3 currentOriginalPosition;
    [SerializeField] private LayerMask raycastLayer;

    public override void Awake()
    {
        base.Awake();
        selectedAxis = new Vector3(1, 0, 0);
    }
    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        mouseState.SetRightclickPress();
        inputManager.OnRightClickDrop += DropInteractable; // Al soltar el clic derecho, limpiamos la interacción

        if (!objective.TryGetComponent<IInteractable>(out IInteractable interactable) || !objective.TryGetComponent<IMovable>(out IMovable component))
            return;
        if ( interactable.IsAtachedToCompass()|| interactable.IsAtachedToRuler() || interactable.IsAtachedToSquad())
            return;
        movable = component;
        movable.ShowOriginFeedback();
        maxRadius = movable.GetMaxRadius();
        currentOriginalPosition = objective.transform.position;
        base.Interact(objective, isPerspective2D);

        inputManager.OnPerspectiveSwitch += DropInteractable;
        inputManager.OnToolSwitchCheck += DropInteractable;
        objectiveRb = objective.GetComponent<Rigidbody>();
        objectiveRb.useGravity = false;
        if (objectiveRb.isKinematic)
        {
            objectiveRb.velocity = Vector3.zero;
            objectiveRb.angularVelocity = Vector3.zero;
        }
        isDragging = true;
        movable.InteractWithCompass();
        
    }

    private void FixedUpdate()
    {
        if (cameraManager.is2D && selectedAxis == Vector3.forward)
            SelectXAxis();
        if (!isDragging) return;
        if (movable.GetIsMovable())
        {
            AdjustDistance(); // Solo ajustar cuando no esté mostrando la rueda de selección
        }
        else
            DropInteractable();
    }


    private void AdjustDistance()
    {
        // Obtener la posición del mouse en el mundo usando Raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, raycastLayer))
        {
            // Posición del mouse en el mundo
            Vector3 mouseWorldPosition = hit.point;

            // Determinar la posición deseada en el eje seleccionado
            float desiredAxisPosition = GetSnappedPosition(mouseWorldPosition);

            // Calcular la nueva posición del objeto en el eje seleccionado
            Vector3 desiredPosition = currentOriginalPosition;
            if (selectedAxis == Vector3.right)
            {
                desiredPosition.x = desiredAxisPosition;
            }
            else if (selectedAxis == Vector3.up)
            {
                desiredPosition.y = desiredAxisPosition;
            }
            else if (selectedAxis == Vector3.forward)
            {
                desiredPosition.z = desiredAxisPosition;
            }

            // Limitar la posición deseada al radio máximo permitido
            Vector3 clampedPosition = ClampToMaxRadius(desiredPosition);

            // Calcular la dirección y distancia para el Raycast
            Vector3 raycastDirection = (clampedPosition - currentOriginalPosition).normalized;
            float raycastDistance = Vector3.Distance(currentOriginalPosition, clampedPosition);

            // Ignorar el propio objeto en el Raycast
            int originalLayer = objective.layer;
            objective.layer = LayerMask.NameToLayer("Ignore Raycast");

            bool hitDetected = Physics.Raycast(currentOriginalPosition, raycastDirection, out RaycastHit rayHit, raycastDistance, raycastLayer);

            // Restaurar la capa original del objeto
            objective.layer = originalLayer;

            if (hitDetected)
            {
                // Ajustar la posición al punto de impacto más un offset hacia el origen
                float offsetDistance = 1.5f; // Ajusta este valor según sea necesario
                Vector3 offsetDirection = (currentOriginalPosition - rayHit.point).normalized;
                Vector3 adjustedPosition = rayHit.point + offsetDirection * offsetDistance;

                // Ajustar al snap más cercano al punto de impacto ajustado
                objective.transform.position = GetSnappedPositionOnAxis(adjustedPosition);
            }
            else
            {
                // Si no hay colisión, mover el objeto al snap más cercano en el eje seleccionado
                objective.transform.position = clampedPosition;
            }
        }
    }

    // Método para ajustar la posición del mouse al snap de la grilla en el eje seleccionado
    private float GetSnappedPosition(Vector3 mouseWorldPosition)
    {
        float cellSize = 1f; // Tamaño de la celda de la grilla

        if (selectedAxis == Vector3.right)
        {
            return Mathf.Round(mouseWorldPosition.x / cellSize) * cellSize;
        }
        else if (selectedAxis == Vector3.up)
        {
            return Mathf.Round(mouseWorldPosition.y / cellSize) * cellSize;
        }
        else if (selectedAxis == Vector3.forward)
        {
            return Mathf.Round(mouseWorldPosition.z / cellSize) * cellSize;
        }

        return 0f;
    }

    // Método para ajustar una posición cualquiera al snap de la grilla en el eje seleccionado
    private Vector3 GetSnappedPositionOnAxis(Vector3 position)
    {
        float cellSize = 1f; // Tamaño de la celda de la grilla
        Vector3 snappedPosition = position;

        if (selectedAxis == Vector3.right)
        {
            snappedPosition.x = Mathf.Round(position.x / cellSize) * cellSize;
        }
        else if (selectedAxis == Vector3.up)
        {
            snappedPosition.y = Mathf.Round(position.y / cellSize) * cellSize;
        }
        else if (selectedAxis == Vector3.forward)
        {
            snappedPosition.z = Mathf.Round(position.z / cellSize) * cellSize;
        }

        return snappedPosition;
    }

    // Método para limitar la posición al radio máximo permitido
    private Vector3 ClampToMaxRadius(Vector3 position)
    {
        Vector3 directionFromOrigin = position - currentOriginalPosition;
        if (directionFromOrigin.magnitude > maxRadius)
        {
            directionFromOrigin = directionFromOrigin.normalized * maxRadius;
        }
        return currentOriginalPosition + directionFromOrigin;
    }


    public override void DropInteractable()
    {
        isDragging = false;
        mouseState.DropRightClick();
        if(movable != null)
        {
            movable.DropWithCompass();
            if(movable.GetNeedGravity())
                objectiveRb.useGravity = true;
        }
        movable = null;
        objectiveRb = null;
        inputManager.OnRightClickDrop -= DropInteractable;
        inputManager.OnPerspectiveSwitch -= DropInteractable;
        inputManager.OnToolSwitchCheck -= DropInteractable;

    }
    public void SelectXAxis()
    {
        selectedAxis = new Vector3(1,0,0);
        MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.CompassXAxis);
    }
    public void SelectYAxis()
    {
        selectedAxis = new Vector3(0, 1, 0);
        MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.CompassYAxis);
    }
    public void SelectZAxis()
    {
        if (cameraManager.is2D)
        {
            selectedAxis = new Vector3(1, 0, 0);
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.CompassXAxis);
        }
        else
        {
            selectedAxis = new Vector3(0, 0, 1);
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.CompassZAxis);
        }
    }
    public override void SetCurrentAlternativeTool()
    {
        if (selectedAxis == Vector3.right)
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.CompassXAxis);
        else if (selectedAxis == Vector3.up)
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.CompassYAxis);
        else if (selectedAxis == Vector3.forward)
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.CompassZAxis);
    }
}
