using UnityEngine;
using UnityEngine.Animations;

public class DragAtachableTool : Tools {
    private bool isDragging = false;
    private GameObject parent;
    private float maxRadius;
    private float initialDistanceMouseToParent;
    private Vector3 initialMousePosition;
    private Vector3 initialObjectPosition;
    public ParentConstraint constraint;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        if(!objective.TryGetComponent<IInteractable>(out  IInteractable interactable))
            return;
        if(!interactable.IsAtachableForCompass())
            return;
        parent = interactable.GetCompassParent();
        if(!parent.TryGetComponent<ICompassable>(out ICompassable compassable))
            return;
        maxRadius = compassable.GetMaxRadius();
        base.Interact(objective, isPerspective2D);
        mouseState.SetRightclickPress();
        constraint = objective.GetComponent<ParentConstraint>();
        inputManager.OnRightClickDrop += DropInteractable; // Al soltar el clic derecho, limpiamos la interacción
        inputManager.OnPerspectiveSwitch += DropInteractable;
        inputManager.OnToolSwitchCheck += DropInteractable;
        isDragging = true;
        initialMousePosition = Input.mousePosition;
        initialDistanceMouseToParent = Vector3.Distance(GetMouseWorldPosition(), parent.transform.position);
        initialObjectPosition = objective.transform.position;
        constraint.enabled = false;
        constraint.locked = false;
        constraint.constraintActive = false;
    }

    private void FixedUpdate()
    {
        if(!isDragging) return;
        AdjustDistance();
    }
    private void AdjustDistance()
    {
        // Obtener la dirección entre el objeto y el padre
        Vector3 direction = (objective.transform.position - parent.transform.position).normalized;

        // Obtener la distancia actual entre el objeto y el padre
        float currentDistanceToParent = Vector3.Distance(objective.transform.position, parent.transform.position);

        // Calcular la nueva distancia basándonos en el desplazamiento del mouse en el eje Y (o cualquier otro control)
        float mouseDeltaY = Input.GetAxis("Mouse Y"); // Cambia esto si usas otro input
        float distanceDelta = mouseDeltaY * 0.1f; // Escalar para controlar la sensibilidad del desplazamiento

        // Calcular la nueva distancia final
        float newDistanceToParent = Mathf.Clamp(currentDistanceToParent + distanceDelta, 2.5f, maxRadius); // Limitar la distancia entre 0.5 y el radio máximo

        // Calcular la nueva posición del objeto basándonos en la nueva distancia
        Vector3 newObjectivePosition = parent.transform.position + direction * newDistanceToParent;

        // Aplicar la nueva posición al objeto
        objective.transform.position = newObjectivePosition;
    }


    private Vector3 GetMouseWorldPosition()
    {
        // Obtener la posición del mouse en el mundo según la perspectiva actual
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (isOn2D)
        {
            // Si estamos en 2D, utilizamos el plano X-Y
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0F));
            newPosition.z = parent.transform.position.z;
            return newPosition;
        }
        else
        {
            // En 2.5D, hacemos un raycast para encontrar la posición en el mundo 3D
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                return hit.point; // Devuelve el punto donde el raycast intersectó un objeto
            }
            else
            {
                Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0F));
                newPosition.z = parent.transform.position.z;
                return newPosition;
            }
        }
    }

    public override void DropInteractable()
    {
        isDragging = false;
        
        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = parent.transform;
        source.weight = 1.0f;
        constraint.SetSource(0,source);
        Vector3 positionOffset = parent.transform.InverseTransformPoint(objective.transform.position);
        Quaternion rotationOffset = Quaternion.Inverse(parent.transform.rotation) * objective.transform.rotation;
        constraint.SetTranslationOffset(0, positionOffset);
        constraint.SetRotationOffset(0, rotationOffset.eulerAngles);

        constraint.constraintActive = true;

        constraint.locked = true; // Mantener el offset original de la relación
        constraint.enabled = true;

        // Limpiar las suscripciones a los eventos
        mouseState.DropRightClick();
        inputManager.OnRightClickDrop -= DropInteractable;
        inputManager.OnPerspectiveSwitch -= DropInteractable;
        inputManager.OnToolSwitchCheck -= DropInteractable;
    }
}
