using UnityEngine;
using UnityEngine.Animations;

public class SetRotationParentTool : Tools
{
    private bool isDragging = false;
    private GameObject firstObject;
    private IRotable rotable;
    private GameObject secondObject;
    private float maxRadius;
    private RotationConstraint constraint;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject interactable, bool isPerspective2D)
    {
        if (!interactable.TryGetComponent<IRotable>(out IRotable rotableComponent))
            return;

        rotable = rotableComponent;

        if (!rotable.CanAttachOthers())
            return;
        if (isDragging)
        {
            ResetConstraint();
        }
        mouseState.SetRightclickPress();
        rotable.OnEraserInteract += ResetDragging;
        rotable.OnEraserInteract += DropInteractable;
        firstObject = interactable;
        maxRadius = rotable.GetMaxRadius();
        isDragging = true;
        playerController.OnRightClickDrop += DropInteractable; // Limpiamos la interacción al soltar clic derecho
        base.Interact(interactable, isPerspective2D);
    }


    private void TryAttachObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            if (!hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable) || !interactable.IsAtachableForSquad())
            {
                isDragging = false;
                return;
            }

            secondObject = hit.collider.gameObject;

            // Chequea la distancia entre los objetos
            float distance = Vector3.Distance(firstObject.transform.position, secondObject.transform.position);
            if (distance > maxRadius)
            {
                isDragging = false ;
                return;
            }
            secondObject.GetComponent<Rigidbody>().isKinematic = true;
            // Agrega el ScaleConstraint
            AddRotationConstraint(secondObject);
        }
        else
            isDragging = false;
    }

    private void AddRotationConstraint(GameObject targetObject)
    {
        if (targetObject.TryGetComponent<RotationConstraint>(out RotationConstraint rotationConstraint))
        {
            isDragging = false;
            return ;
        }
        constraint = targetObject.AddComponent<RotationConstraint>();
        // Guardamos la escala inicial del objeto antes de agregar el constraint
        Vector3 initialTargetScale = targetObject.transform.localScale;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = firstObject.transform;
        source.weight = 1.0f;
        constraint.AddSource(source);
        constraint.constraintActive = true;
        constraint.rotationOffset = secondObject.transform.rotation.eulerAngles - firstObject.transform.rotation.eulerAngles;

    }

    public override void DropInteractable()
    {
        if (isDragging)
        {
            TryAttachObject();
        }
        if (!isDragging && secondObject != null)
        {
            ResetConstraint();
            firstObject = null;
            secondObject.GetComponent<Rigidbody>().isKinematic = false;
            secondObject = null;
            if(rotable != null)
            {
                rotable.OnEraserInteract -= ResetDragging;
                rotable.OnEraserInteract -= DropInteractable;
                rotable = null;
            }
        }
        mouseState.DropRightClick();
        base.DropInteractable();
        playerController.OnRightClickDrop -= DropInteractable;
    }
    private void ResetDragging()
    {
        isDragging = false;
    }
    private void ResetConstraint()
    {
        if (constraint == null)
            return;
        // Desactivar el constraint y limpiar la fuente
        constraint.enabled = false;
        constraint.RemoveSource(0);
        Destroy(constraint);
        constraint = null;

    }
}
