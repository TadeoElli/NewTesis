using UnityEngine;

public class PaintableDelete : PaintableObject
{


    private void Awake()
    {
        base.InteractionWithBrush();
    }
    public override void InteractionWithBrush()
    {

    }
    public override void InteractionWithEraser()
    {
        base.InteractionWithEraser();
        Destroy(gameObject);
    }
}
