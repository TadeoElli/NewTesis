using UnityEngine;

public class PaintableSolid : PaintableObject
{
    [SerializeField] private Collider col;
    [SerializeField] private Material solidMat, transparentMat;
    [SerializeField] private Color feedbackColor;
    [SerializeField] private Renderer render;
    [SerializeField] private bool startSolid;

    private void Awake()
    {
        if (startSolid)
            InteractionWithBrush();
        else
            InteractionWithEraser();
    }
    public override void InteractionWithBrush()
    {
        base.InteractionWithBrush();
        col.isTrigger = false;
        render.material = solidMat;
    }
    public override void InteractionWithEraser()
    {
        base.InteractionWithEraser();
        col.isTrigger = true;
        render.material = transparentMat;
    }
    public override void HideFeedback()
    {
        FeedbackManager.Instance.ApplyFeedback(base.objRenderer, feedbackColor);
    }
}
