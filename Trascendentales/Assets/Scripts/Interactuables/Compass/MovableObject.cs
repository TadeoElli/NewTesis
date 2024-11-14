using System;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(InteractuableObject))]
public class MovableObject : MonoBehaviour, IMovable,IFeedback
{
    [SerializeField] private float maxRadius;
    private Vector3 originalPosition;
    private Color m_feedbackCompass, m_feedbackEraser;
    [SerializeField] private Renderer objRenderer;

    public event Action OnEraserInteract;

    public float GetMaxRadius() => maxRadius;

    private void Start()
    {
        m_feedbackCompass = ColorDictionary.GetColor("FeedbackCompass");
        m_feedbackEraser = ColorDictionary.GetColor("FeedbackEraser");
        originalPosition = transform.position;
        OnEraserInteract += ResetPosition;
    }
    public Vector3 GetOriginalPosition() => originalPosition;

    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }
    public void ShowFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Compass &&
            !MouseState.Instance.IsLeftClickPress())
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackCompass);
        else if (MouseState.Instance.CurrentToolActive() == ToolTypes.Eraser &&
            !MouseState.Instance.IsLeftClickPress() &&
            !MouseState.Instance.IsRightClickPress())
            FeedbackManager.Instance.ApplyFeedback(objRenderer, m_feedbackEraser);


    }
    private void ResetPosition()
    {
        transform.position = originalPosition;
    }
    public void HideFeedback()
    {
        if (MouseState.Instance.CurrentToolActive() == ToolTypes.Compass &&
            MouseState.Instance.IsRightClickPress())
            return;
        Debug.Log("Sali");
        FeedbackManager.Instance.ClearFeedback(objRenderer);
    }


}
