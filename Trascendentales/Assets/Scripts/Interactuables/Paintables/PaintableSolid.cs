using UnityEngine;

public class PaintableSolid : PaintableObject
{
    [SerializeField] private Collider col;
    [SerializeField] private Material solidMat, transparentMat;
    [SerializeField] private Renderer render;

    private void Awake()
    {
        InteractionWithBrush();
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
}
