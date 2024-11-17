using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InteractuableObject))]
public class RotableObject : MonoBehaviour, IRotable, IFeedback
{
    [SerializeField] private bool canRotateInY;
    [SerializeField] private bool canRotateInZ;
    [SerializeField] private bool canRotate = true;
    [SerializeField] private float maxRadius;
    [SerializeField] private bool canAttachOthers;
    [SerializeField] private bool needToBeKinematic = false;
    [SerializeField] public bool isRotating = false;
    [SerializeField] public Transform gimballRef;
    private Quaternion originalRotation;
    public event Action OnEraserInteract;
    private Color m_feedback;
    [SerializeField] private Renderer objRenderer;



    public bool CanRotate() => canRotate;
    public bool NeedToBeKinematic() => needToBeKinematic;
    public bool CanRotateInY() => canRotateInY;
    public bool CanRotateInZ() => canRotateInZ;
    public bool CanAttachOthers() => canAttachOthers;
    public float GetMaxRadius() => maxRadius;
    public bool IsRotating() => isRotating;

    public void SetIsRotating(bool state)
    {
        isRotating = state;
    }

    private void Start()
    {
        m_feedback = ColorDictionary.GetColor("FeedbackSquad");
        originalRotation = transform.rotation;
        OnEraserInteract += ResetRotation;
    }
    public void SetCanRotate(bool state)
    {
        canRotate = state;
        if(!canRotate)
            m_feedback = new Color(0,0,0, 0);
    }
    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }
    public void SetGimballRef(Transform newRef)
    {
        gimballRef = newRef;
    }

    public void SetRotationConstraints(bool canRotateInY, bool canRotateInZ)
    {
        this.canRotateInY = canRotateInY;
        this.canRotateInZ = canRotateInZ;
    }


    public void ShowFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Squad &&
            !MouseState.Instance.IsLeftClickPress() &&
            !MouseState.Instance.IsRightClickPress()
            )
        {
            // Aplicar feedback si la herramienta activa es la regla y no hay clicks activos
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedback);
        }


    }

    private void ResetRotation()
    {
        transform.rotation = originalRotation;
    }

    public void HideFeedback()
    {
        FeedbackManager.Instance.ClearFeedback(objRenderer); // Limpiar el feedback al salir del objeto
    }
}
