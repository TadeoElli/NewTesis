using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CompassTool : Tools
{
    [SerializeField] private Transform gimball;
    [SerializeField] private float maxRadius; // Radio máximo permitido
    [SerializeField] private LayerMask interactableLayer;
    private ParentConstraint parentConstraint; // El constraint para la rotación
    private Vector3 initialMousePosition;
    private GameObject firstObject;
    private GameObject secondObject;
    private float currentRadius;
    private bool isDragging = false;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject interactable, bool isPerspective2D)
    {
        playerController.OnToolDesinteract += TryAttachObject;
        playerController.OnToolDesinteract += DropInteractable;
        if(!interactable.TryGetComponent<ICompassable>(out ICompassable component))
            return;
        if (isDragging)
        {
            ResetConstraint();
        }
        ResetGimball();
        gimball.SetParent(interactable.transform);
        firstObject = gimball.gameObject;
        maxRadius = component.GetMaxRadius();
        base.Interact(interactable, isPerspective2D);
        //firstObject.InteractWithCompass(); // Accionar la interacción del primer objeto
        isDragging = true;
        initialMousePosition = Input.mousePosition;
    }

    void Update()
    {
        if (!isDragging) return;

        // Mientras se mantenga el clic, dibujar la circunferencia y ajustar el radio
        Vector3 currentMousePosition = Input.mousePosition;
        float distance = Vector3.Distance(initialMousePosition, currentMousePosition);
        currentRadius = Mathf.Min(distance * 0.01f, maxRadius); // Ajusta el radio y lo clampa al máximo

        // Dependiendo de la perspectiva, dibuja la circunferencia en el eje correcto
        if (isOn2D)
        {
            // En 2D, circunferencia en el eje Z
            Debug.DrawLine(firstObject.transform.position, firstObject.transform.position + Vector3.right * currentRadius, Color.red);
        }
        else
        {
            // En 2.5D, circunferencia en el eje Y
            Debug.DrawLine(firstObject.transform.position, firstObject.transform.position + Vector3.forward * currentRadius, Color.red);
        }
    }

    private void TryAttachObject()
    {
        // Hacer un raycast para ver si el jugador suelta el clic sobre otro objeto interactuable
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            if (!hit.collider.TryGetComponent<IInteractable>(out IInteractable component))
                return;
            // Si el objeto es interactuable, atachear al gimball
            if (!component.IsAtachable())
            {
                isDragging = false;
                return;
            }

            secondObject = hit.collider.gameObject;
            //secondObject.InteractWithCompass();

            // Configurar el constraint para que el segundo objeto siga la rotación del gimball
            if (parentConstraint != null)
                return;
            ParentConstraint constraint = secondObject.AddComponent<ParentConstraint>();
            parentConstraint = constraint;
            ConstraintSource source = new ConstraintSource();
            source.sourceTransform = firstObject.transform;
            source.weight = 1.0f;
            parentConstraint.AddSource(source);
            parentConstraint.SetTranslationOffset(0, secondObject.transform.position - firstObject.transform.position);
            parentConstraint.SetRotationOffset(0, secondObject.transform.rotation.eulerAngles - firstObject.transform.rotation.eulerAngles);
            parentConstraint.constraintActive = true;

            parentConstraint.locked = true; // Mantener el offset original de la relación
        }
        else
            isDragging = false;
    }

    public override void DropInteractable()
    {
        if (!isDragging)
        {
            if (firstObject != null)
            {
                //firstObject.DropWithCompass();
                firstObject = null;
            }
            if (secondObject != null)
            {
                //secondObject.DropWithCompass();
                secondObject = null;
            }
        }
        playerController.OnToolDesinteract -= TryAttachObject;
        playerController.OnToolDesinteract -= DropInteractable;
        base.DropInteractable();
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
        // Desactivar el constraint y limpiar la fuente
        parentConstraint.constraintActive = false;
        parentConstraint.RemoveSource(0);
        parentConstraint.enabled = false;
        parentConstraint = null;
        Destroy(secondObject.GetComponent<ParentConstraint>());
    }
}

