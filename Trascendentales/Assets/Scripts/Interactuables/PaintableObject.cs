using UnityEngine;
[RequireComponent (typeof(InteractuableObject))]
public class PaintableObject : MonoBehaviour, IPaintable
{
    [SerializeField] private Vector3 oldPosition;
    [SerializeField] private Vector3 newPosition;
    [SerializeField] private InteractionsByPerspectiveTypes interactionForBrush, interactionForEraser;
    private bool wasInteracted = false;
    public void DropWithBrush(bool isOn2D)
    {
    }

    public void DropWithEraser(bool isOn2D)
    {
    }

    public void InteractWithBrush(bool isOn2D)
    {
        if(wasInteracted)
            return;
        switch (interactionForBrush)
        {
            case InteractionsByPerspectiveTypes.BothPerspectives:
                InteractionWithBrush();
                break;
            case InteractionsByPerspectiveTypes.Only2D:
                if(isOn2D)
                    InteractionWithBrush();
                break;
            case InteractionsByPerspectiveTypes.Only3D:
                if(!isOn2D)
                    InteractionWithBrush();
                break;
            default:
                break;
        }

    }
    public void InteractWithEraser(bool isOn2D)
    {
        if (!wasInteracted)
            return;
        switch (interactionForEraser)
        {
            case InteractionsByPerspectiveTypes.BothPerspectives:
                InteractionWithEraser();
                break;
            case InteractionsByPerspectiveTypes.Only2D:
                if (isOn2D)
                    InteractionWithEraser();
                break;
            case InteractionsByPerspectiveTypes.Only3D:
                if (!isOn2D)
                    InteractionWithEraser();
                break;
            default:
                break;
        }
    }

    public virtual void InteractionWithBrush()
    {
        transform.position = newPosition;
        wasInteracted = true;
    }
    public virtual void InteractionWithEraser()
    {
        transform.position = oldPosition;
        wasInteracted = false;
    }
}
public enum InteractionsByPerspectiveTypes
{
    BothPerspectives,
    Only2D,
    Only3D
}