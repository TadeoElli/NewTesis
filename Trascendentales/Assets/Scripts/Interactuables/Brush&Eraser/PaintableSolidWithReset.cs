using System.Collections;
using UnityEngine;

public class PaintableSolidWithReset : PaintableSolid
{
    [SerializeField] private float resetDelay = 1f; // Tiempo en segundos antes de activar la otra interacción
    private enum DefaultState{ Solid, Transparent }
    [SerializeField] private DefaultState state;


    public override void InteractionWithBrush()
    {
        base.InteractionWithBrush();
        if (state == DefaultState.Transparent)
            StartCoroutine(ResetToEraserAfterDelay());
    }
    public override void InteractionWithEraser()
    {
        base.InteractionWithEraser();
        if (state == DefaultState.Solid)
            StartCoroutine(ResetToBrushAfterDelay());
    }
    private IEnumerator ResetToBrushAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        InteractionWithBrush();
    }
    private IEnumerator ResetToEraserAfterDelay()
    {
        yield return new WaitForSeconds(resetDelay);
        InteractionWithEraser();
    }
}
