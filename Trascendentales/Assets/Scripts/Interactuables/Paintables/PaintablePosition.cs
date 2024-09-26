using UnityEngine;

public class PaintablePosition : PaintableObject
{
    [SerializeField] private Vector3 oldPosition;
    [SerializeField] private Vector3 newPosition;

    public override void InteractionWithBrush()
    {
        base.InteractionWithBrush();
        transform.position = newPosition;
    }
    public override void InteractionWithEraser()
    {
        base.InteractionWithEraser();
        transform.position = oldPosition;
    }
}
