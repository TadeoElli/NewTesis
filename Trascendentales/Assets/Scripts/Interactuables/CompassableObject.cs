using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InteractuableObject))]
public class CompassableObject : MonoBehaviour, ICompassable, IFeedback
{
    [SerializeField]private float maxRadius;
    [SerializeField]private Color m_feedbackCompass, m_feedbackEraser;
    [SerializeField] private Renderer objRenderer;
    public event Action OnEraserInteract;
    public event Action OnEraserDrop;

    public float GetMaxRadius() => maxRadius;

    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
        OnEraserInteract = null;
    }
    public void DropWithEraser(bool isOn2D)
    {
        OnEraserDrop?.Invoke();
    }


    public void SetMaxRadius(float maxRadius)
    {
        this.maxRadius = maxRadius;
    }

    public void ShowFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Compass &&
            !MouseState.Instance.IsRightClickPress())
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackCompass);
        else if(MouseState.Instance.CurrentToolActive() == ToolTypes.Eraser &&
            !MouseState.Instance.IsLeftClickPress() &&
            !MouseState.Instance.IsRightClickPress())
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackEraser);


    }

    public void HideFeedback()
    {
        if(MouseState.Instance.CurrentToolActive() == ToolTypes.Compass &&
            MouseState.Instance.IsLeftClickPress())
            return;
        Debug.Log("Sali");
        FeedbackManager.Instance.ClearFeedback(objRenderer);
    }
}

