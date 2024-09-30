using UnityEngine;
using UnityEngine.Animations;

public class ScaleAtachableTool : Tools
{
    private bool isDragging = false;
    private GameObject firstObject;
    private IEscalable scalable;
    private GameObject secondObject;
    private float maxRadius;
    private Vector3 initialScale;
    private ScaleConstraint constraint;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject interactable, bool isPerspective2D)
    {
        if (!interactable.TryGetComponent<IEscalable>(out IEscalable scalableComponent))
            return;

        scalable = scalableComponent;

        if (!scalable.CanAttachOthers())
            return;
        if (isDragging)
        {
            ResetConstraint();
        }
        scalable.OnEraserInteract += ResetDragging;
        scalable.OnEraserInteract += DropInteractable;
        firstObject = interactable;
        maxRadius = scalable.GetMaxRadius();
        initialScale = firstObject.transform.localScale;
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
            if (!hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable) || !interactable.IsAtachableForRuler())
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

            // Agrega el ScaleConstraint
            AddScaleConstraint(secondObject);
        }
        else
            isDragging = false;
    }

    private void AddScaleConstraint(GameObject targetObject)
    {
        if (targetObject.TryGetComponent<ScaleConstraint>(out ScaleConstraint scaleConstraint))
        {
            isDragging = false;
            return ;
        }
        constraint = targetObject.AddComponent<ScaleConstraint>();
        // Guardamos la escala inicial del objeto antes de agregar el constraint
        Vector3 initialTargetScale = targetObject.transform.localScale;

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = firstObject.transform;
        source.weight = 1.0f;
        constraint.AddSource(source);
        constraint.constraintActive = true;
        // Calcular el "ScaleOffset" multiplicando la escala inicial del target por el inverso de la escala del primer objeto
        Vector3 inverseFirstScale = new Vector3(
            1.0f / firstObject.transform.localScale.x,
            1.0f / firstObject.transform.localScale.y,
            1.0f / firstObject.transform.localScale.z
        );
        Vector3 scaleOffset = Vector3.Scale(initialTargetScale, inverseFirstScale);
        constraint.scaleOffset = scaleOffset;


        // Aplica la escala actual
        //targetObject.transform.localScale = firstObject.transform.localScale;
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
            secondObject = null;
            if(scalable != null)
            {
                scalable.OnEraserInteract -= ResetDragging;
                scalable.OnEraserInteract -= DropInteractable;
                scalable = null;
            }
        }
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
        // Guardar la escala actual del objeto antes de eliminar el constraint
        //Vector3 currentScale = secondObject.transform.localScale;
        // Desactivar el constraint y limpiar la fuente
        constraint.enabled = false;
        constraint.RemoveSource(0);
        Destroy(constraint);
        constraint = null;
        // Aplicar la escala guardada
        //secondObject.transform.localScale = currentScale;
    }
}

