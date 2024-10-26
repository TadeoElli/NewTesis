using System.Collections.Generic;
using UnityEngine;

public class PaintableEnablescale : PaintableObject
{
    [SerializeField] private List<PaintableEnablescale> otherParts;
    [SerializeField] private ScalableObject scalable;

//
    public override void InteractionWithBrush()
    {
        base.InteractionWithBrush();
        foreach (var part in otherParts)
        {
            part.InteractionWithEraser();
        }
        if(scalable != null)
            scalable.canScale = true;
    }
    public override void InteractionWithEraser()
    {
        base.InteractionWithEraser();
        if(scalable != null)
            scalable.canScale = false;
    }
}
