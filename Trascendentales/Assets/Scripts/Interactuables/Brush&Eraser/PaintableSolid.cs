using UnityEngine;
using UnityEngine.Events;


public class PaintableSolid : PaintableObject
{
    [SerializeField] private Collider col;
    private Color feedbackColor;
    [SerializeField] private bool startSolid;
    public UnityEvent OnInteractionWitBrush, OnInteractionWithEraser;
    public override void Awake()
    {
        base.Awake();
        if (startSolid)
            InteractionWithBrush();
        else
            InteractionWithEraser();
        feedbackColor = Color.black;
    }
    public override void InteractionWithBrush()
    {
        base.InteractionWithBrush();
        col.isTrigger = false;
        OnInteractionWitBrush?.Invoke();
    }
    public override void InteractionWithEraser()
    {
        base.InteractionWithEraser();
        col.isTrigger = true;
        OnInteractionWithEraser?.Invoke();
    }
    public override void HideFeedback()
    {
        FeedbackManager.Instance.ApplyFeedback(base.objRenderer, feedbackColor);
    }
}
