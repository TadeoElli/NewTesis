using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.Animations;

public class CompassTool : Tools
{
    [SerializeField] private Transform gimball;
    [SerializeField] private GameObject compassGizmo;
    private float maxRadius; // Radio máximo permitido
    private ParentConstraint parentConstraint; // El constraint para la rotación
    private Vector3 initialMousePosition;
    private GameObject firstObject;
    private ICompassable compassable;
    private GameObject secondObject;
    private float currentRadius;
    private bool isDragging = false;
    private bool wasClicked = false;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject interactable, bool isPerspective2D)
    {
        inputManager.OnLeftClickDrop += DropInteractable;
        cameraManager.OnCameraSwitch += ResetDragging;
        cameraManager.OnCameraSwitch += DropInteractable;
        if(!interactable.TryGetComponent<ICompassable>(out ICompassable component))
            return;
        mouseState.SetLeftclickPress();
        compassable = component;
        compassable.OnEraserInteract += ResetDragging; // Accionar la interacción del primer objeto
        compassable.OnEraserInteract += DropInteractable; // Accionar la interacción del primer objeto
        if (isDragging)
        {
            ResetConstraint();
        }
        ResetGimball();
        gimball.position = interactable.transform.position;
        gimball.SetParent(interactable.transform);
        firstObject = gimball.gameObject;
        maxRadius = component.GetMaxRadius();
        base.Interact(interactable, isPerspective2D);
        compassGizmo.SetActive(true);
        compassGizmo.transform.position = gimball.position;
        if(isOn2D) compassGizmo.transform.rotation = Quaternion.Euler(0,0,0);
        else compassGizmo.transform.rotation = Quaternion.Euler(-90, 0, 0);
        isDragging = true;
        wasClicked = true;
        initialMousePosition = Input.mousePosition;
        FeedbackManager.Instance.StartMouseLine(objective);
    }

    void FixedUpdate()
    {
        if (!isDragging || !wasClicked) return;
        // Mientras se mantenga el clic, dibujar la circunferencia y ajustar el radio
        Vector3 currentMousePosition = Input.mousePosition;
        //FeedbackManager.Instance.ActivateLineRenderer(firstObject, currentMousePosition);
        float distance = Vector3.Distance(initialMousePosition, currentMousePosition);
        currentRadius = Mathf.Min(distance * 0.01f, maxRadius); // Ajusta el radio y lo clampa al máximo
        compassGizmo.transform.localScale = new Vector3(currentRadius /1.5f, currentRadius /1.5f, 0.1f );
        // Dependiendo de la perspectiva, dibuja la circunferencia en el eje correcto
        if (isOn2D)
        {
            // En 2D, circunferencia en el eje Z
            Debug.DrawLine(firstObject.transform.position, firstObject.transform.position + Vector3.right * currentRadius, Color.red);
            if(parentConstraint != null)
                parentConstraint.translationAxis = Axis.X | Axis.Y;
        }
        else
        {
            // En 2.5D, circunferencia en el eje Y
            Debug.DrawLine(firstObject.transform.position, firstObject.transform.position + Vector3.forward * currentRadius, Color.red);
            if(parentConstraint != null)
                parentConstraint.translationAxis = Axis.X | Axis.Z;
        }
    }

    private void TryAttachObject()
    {
        // Hacer un raycast para ver si el jugador suelta el clic sobre otro objeto interactuable
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        FeedbackManager.Instance.StopMouseLine();

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            if (!hit.collider.TryGetComponent<IInteractable>(out IInteractable component))
            {
                isDragging = false;
                return;
            }
            // Si el objeto es interactuable, atachear al gimball
            if (!component.IsAtachableForCompass() || hit.collider.gameObject == objective)
            {
                isDragging = false;
                return;
            }
            component.SetIsAtachedToCompass(objective);

            secondObject = hit.collider.gameObject;
            secondObject.GetComponent<Rigidbody>().isKinematic = true;

            // Configurar el constraint para que el segundo objeto siga la rotación del gimball
            if (parentConstraint != null)
            {
                isDragging = false;
                return;
            }
            ParentConstraint constraint = secondObject.AddComponent<ParentConstraint>();
            parentConstraint = constraint;
            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = firstObject.transform;
            source.weight = 1.0f;
            parentConstraint.AddSource(source);
            parentConstraint.SetTranslationOffset(0, secondObject.transform.position - firstObject.transform.position);
            parentConstraint.SetRotationOffset(0, secondObject.transform.rotation.eulerAngles - firstObject.transform.rotation.eulerAngles);
            if (!component.IsRotatingTowardsTheCompass())
                parentConstraint.rotationAxis = Axis.None;
            parentConstraint.constraintActive = true;

            parentConstraint.locked = true; // Mantener el offset original de la relación
            FeedbackManager.Instance.ActivateLineRenderer(firstObject, secondObject);

            
        }
        else
        {
            isDragging = false;
        }
    }

    public override void DropInteractable()
    {
        wasClicked = false;
        if (isDragging)
            TryAttachObject();        
        if (!isDragging)
        {
            ResetConstraint();
            if (firstObject != null)
            {
                firstObject = null;
            }
            if (secondObject != null)
            {
                secondObject.GetComponent<Rigidbody>().isKinematic = false;
                if (secondObject.TryGetComponent<IInteractable>(out IInteractable component))
                {
                    component.SetUnatachedToCompass();
                }
                secondObject = null;
            }
            if (compassable != null) { 
            compassable.OnEraserInteract -= ResetDragging;
            compassable.OnEraserInteract -= DropInteractable;
            compassable = null;
            }
            ResetGimball();
            compassGizmo.SetActive(false);
        }
        base.DropInteractable();
        mouseState.DropLeftClick();
        inputManager.OnLeftClickDrop -= DropInteractable;
    }
    private void ResetDragging()
    {
        isDragging = false;
        FeedbackManager.Instance.DeactivateLineRenderer();
        cameraManager.OnCameraSwitch -= ResetDragging;
        cameraManager.OnCameraSwitch -= DropInteractable;
    }
    private void ResetGimball()
    {
        gimball.SetParent(null);
        gimball.position = Vector3.zero;
        gimball.rotation = Quaternion.identity;
        gimball.localScale = Vector3.one;
    }
    private void ResetConstraint()
    {
        currentRadius = 0f;
        if (parentConstraint == null)
            return;
        // Desactivar el constraint y limpiar la fuente
        FeedbackManager.Instance.DeactivateLineRenderer();
        parentConstraint.constraintActive = false;
        parentConstraint.RemoveSource(0);
        parentConstraint.enabled = false;
        parentConstraint = null;
        Destroy(secondObject.GetComponent<ParentConstraint>());
    }
}
