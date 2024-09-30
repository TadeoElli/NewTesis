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
        constraint = objective.GetComponent<ParentConstraint>();
        playerController.OnRightClickDrop += DropInteractable; // Al soltar el clic derecho, limpiamos la interacción
        playerController.OnPerspectiveSwitch += DropInteractable;
        playerController.OnToolSwitchCheck += DropInteractable;
        isDragging = true;
        initialMousePosition = Input.mousePosition;
        initialDistanceMouseToParent = Vector3.Distance(GetMouseWorldPosition(), parent.transform.position);
        initialObjectPosition = objective.transform.position;
        constraint.enabled = false;
    }

    private void Update()
    {
        if(!isDragging) return;
        AdjustDistance();
    }
    private void AdjustDistance()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 currentMousePosition = GetMouseWorldPosition();

        // Calcular la nueva distancia del mouse al objeto padre
        float newDistanceMouseToParent = Vector3.Distance(currentMousePosition, parent.transform.position);

        // Calcular el cambio de distancia entre el mouse y el padre
        float distanceDelta = newDistanceMouseToParent - initialDistanceMouseToParent;

        // Calcular la nueva posición del objeto objetivo basado en el cambio de distancia
        Vector3 direction = (objective.transform.position - parent.transform.position).normalized;
        Vector3 newObjectivePosition = initialObjectPosition + direction * distanceDelta;

        // Restringir la distancia del objetivo al objeto padre
        float distanceToParent = Vector3.Distance(newObjectivePosition, parent.transform.position);
        if (distanceToParent > maxRadius)
        {
            newObjectivePosition = parent.transform.position + direction * maxRadius;
        }
        else if (distanceToParent < 0.5f) // No permitir que se superpongan
        {
            newObjectivePosition = parent.transform.position + direction * 0.5f; // Distancia mínima
        }
        // Aplicar la nueva posición al objeto objetivo
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
        constraint.SetTranslationOffset(0, objective.transform.position - parent.transform.position);
        constraint.SetRotationOffset(0, objective.transform.rotation.eulerAngles - parent.transform.rotation.eulerAngles);
        constraint.constraintActive = true;

        constraint.locked = true; // Mantener el offset original de la relación
        constraint.enabled = true;

        // Limpiar las suscripciones a los eventos
        playerController.OnRightClickDrop -= DropInteractable;
        playerController.OnPerspectiveSwitch -= DropInteractable;
        playerController.OnToolSwitchCheck -= DropInteractable;
    }
}
