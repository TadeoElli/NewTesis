using UnityEngine;
[RequireComponent (typeof(InteractuableObject))]
public class PaintableObject : MonoBehaviour, IPaintable, IFeedback
{
    CameraManager cameraManager;
    [SerializeField] private InteractionsByPerspectiveTypes interactionForBrush, interactionForEraser;
    private Color m_feedbackBrush, m_feedbackEraser;
    [SerializeField] protected Renderer objRenderer;
    private bool wasInteracted = false;
    public virtual void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
    }
    private void Start()
    {
        m_feedbackBrush = ColorDictionary.GetColor("FeedbackBrush");
        m_feedbackEraser = ColorDictionary.GetColor("FeedbackEraser");
    }

    public void InteractWithBrush(bool isOn2D)
    {
        if(wasInteracted)
            return;
        InteractionWithBrush(); 
    }
    public void InteractWithEraser(bool isOn2D)
    {
        if (!wasInteracted)
            return;
        InteractionWithEraser();
    }

    public virtual void InteractionWithBrush()
    {
        wasInteracted = true;
    }
    public virtual void InteractionWithEraser()
    {
        wasInteracted = false;
    }

    public bool CanInteractWithBrush(bool isOn2D)
    {
        return CheckInteractions(interactionForBrush, isOn2D);
    }
    public bool CanInteractWithEraser(bool isOn2D)
    {
        return CheckInteractions(interactionForEraser, isOn2D);
    }
    public bool CheckInteractions(InteractionsByPerspectiveTypes type, bool isOn2D)
    {
        switch (type)
        {
            case InteractionsByPerspectiveTypes.BothPerspectives:
                return true;
            case InteractionsByPerspectiveTypes.Only2D:
                if (isOn2D) return true;
                else return false;
            case InteractionsByPerspectiveTypes.Only3D:
                if (!isOn2D) return true;
                else return false;
            default:
                return false;
        }
    }

    public virtual void ShowFeedback()
    {
        if(MouseState.Instance.CurrentToolActive() == ToolTypes.Brush &&
            !MouseState.Instance.IsLeftClickPress() &&
            !MouseState.Instance.IsRightClickPress() &&
             !wasInteracted && CheckInteractions(interactionForBrush, cameraManager.is2D))
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackBrush);
        else if (MouseState.Instance.CurrentToolActive() == ToolTypes.Eraser )
            if(!MouseState.Instance.IsLeftClickPress())
                if(!MouseState.Instance.IsRightClickPress())
                    if(wasInteracted)
                        if(CheckInteractions(interactionForEraser, cameraManager.is2D))
                            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackEraser);

    }

    public virtual void HideFeedback()
    {
        FeedbackManager.Instance.ClearFeedback(objRenderer); // Limpiar el feedback al salir del objeto
    }
}
public enum InteractionsByPerspectiveTypes
{
    BothPerspectives,
    Only2D,
    Only3D
}