using UnityEngine;

public class PaintableDelete : PaintableObject
{
    [SerializeField] private bool needToDestroy = false;

    public override void InteractWithEraser(bool isOn2D)
    {
        InteractionWithEraser();
    }
    public override void InteractionWithEraser()
    {
        if (needToDestroy)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
