using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class MoveObjectTool : Tools
{
    private bool isDragging = false;
    private Rigidbody objectiveRb;
    private float maxRadius;
    private float initialDistanceMouseToParent;
    private IMovable movable;
    private Vector3 selectedAxis; // Nuevo: Almacena el eje seleccionado
    private Vector3 originalPosition;
    private Vector3 currentOriginalPosition;
    private Vector3 currentOffset;
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
        if (!interactable.IsAtachableForCompass() || interactable.IsAtachedToCompass()|| interactable.IsAtachedToRuler() || interactable.IsAtachedToSquad())
            return;
        movable = component;
        movable.ShowOriginFeedback();
        maxRadius = movable.GetMaxRadius();
        originalPosition = movable.GetOriginalPosition();
        currentOriginalPosition = objective.transform.position;
        currentOffset = Vector3.zero;

        base.Interact(objective, isPerspective2D);

        inputManager.OnPerspectiveSwitch += DropInteractable;
        inputManager.OnToolSwitchCheck += DropInteractable;
        objectiveRb = objective.GetComponent<Rigidbody>();
        objectiveRb.useGravity = false;
        objectiveRb.velocity = Vector3.zero;
        objectiveRb.angularVelocity = Vector3.zero;
        isDragging = true;
        
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
        // Calcular el desplazamiento actual y la distancia desde el origen en el eje seleccionado
        Vector3 currentOffset = objective.transform.position - currentOriginalPosition;
        float currentDistanceToOrigin = Vector3.Dot(currentOffset, selectedAxis);

        // Obtener el desplazamiento del mouse en el eje seleccionado (positivo o negativo)
        float mouseDelta;
        if (selectedAxis == Vector3.right)
            if (!cameraManager.isFrontView)
                mouseDelta = Input.GetAxis("Mouse X") * -1f;
            else
                mouseDelta = Input.GetAxis("Mouse X");
        else if(selectedAxis == Vector3.forward)
            if (!cameraManager.isFrontView)
                mouseDelta = Input.GetAxis("Mouse Y") * -1f;
            else
                mouseDelta = Input.GetAxis("Mouse Y");
        else
            mouseDelta = Input.GetAxis("Mouse Y");

        // Calcular la nueva distancia objetivo y clamping entre los límites
        float newDistanceToOrigin = Mathf.Clamp(currentDistanceToOrigin + mouseDelta, -maxRadius, maxRadius);
        Vector3 desiredPosition = currentOriginalPosition + selectedAxis * newDistanceToOrigin;

        // Clamping para asegurar que no exceda el radio máximo
        if (Vector3.Distance(originalPosition, desiredPosition) > maxRadius)
        {
            desiredPosition = originalPosition + (desiredPosition - originalPosition).normalized * maxRadius;
        }


        // Determinar la dirección del Raycast basándose en si el desplazamiento es positivo o negativo
        Vector3 raycastDirection = (desiredPosition - originalPosition).normalized;
        float raycastDistance = Vector3.Distance(originalPosition, desiredPosition);

        // Ignorar el propio objeto en el Raycast
        int originalLayer = objective.layer;
        objective.layer = LayerMask.NameToLayer("Ignore Raycast");

        bool hitDetected = Physics.Raycast(originalPosition, raycastDirection, out RaycastHit hit, raycastDistance, raycastLayer);

        // Restaurar la capa original del objeto
        objective.layer = originalLayer;

        if (hitDetected)
        {
            Debug.Log(hit.collider.gameObject);
            // Calcular un offset en dirección hacia la posición original
            float offsetDistance = 1.5f; // Puedes ajustar este valor para definir la distancia del offset
            Vector3 offsetDirection = (originalPosition - hit.point).normalized;
            Vector3 newPosition = hit.point + offsetDirection * offsetDistance;

            // Establecer la posición del objeto en el punto de impacto más el offset
            objective.transform.position = newPosition;
        }
        else
        {
            // Si no hay colisión, mover el objeto a la posición deseada
            objective.transform.position = desiredPosition;
        }
    }


    public override void DropInteractable()
    {
        isDragging = false;
        mouseState.DropRightClick();
        if(movable.GetNeedGravity())
            objectiveRb.useGravity = true;
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
