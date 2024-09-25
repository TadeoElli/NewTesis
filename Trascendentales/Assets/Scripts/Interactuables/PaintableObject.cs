using UnityEngine;
[RequireComponent (typeof(InteractuableObject))]
public class PaintableObject : MonoBehaviour, IPaintable
{
    [SerializeField] private Vector3 oldPosition;
    [SerializeField] private Vector3 newPosition;
    [SerializeField] private InteractionsByPerspectiveTypes interactionForBrush, interactionForEraser;
    private bool wasInteracted = false;


    public void InteractWithBrush(bool isOn2D)
    {
        if(wasInteracted)
            return;
        InteractionWithBrush();
    }
    public void InteractWithEraser(bool isOn2D)
    {
        if (!wasInteracted)
            return;
        InteractionWithEraser();
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

    public bool CanInteractWithBrush(bool isOn2D)
    {
        return CheckInteractions(interactionForBrush, isOn2D);
    }
    public bool CanInteractWithEraser(bool isOn2D)
    {
        return CheckInteractions(interactionForEraser, isOn2D);
    }
    private bool CheckInteractions(InteractionsByPerspectiveTypes type, bool isOn2D)
    {
        switch (type)
        {
            case InteractionsByPerspectiveTypes.BothPerspectives:
                return true;
            case InteractionsByPerspectiveTypes.Only2D:
                if (isOn2D) return true;
                else return false;
            case InteractionsByPerspectiveTypes.Only3D:
                if (!isOn2D) return true;
                else return false;
            default:
                return false;
        }
    }
}
public enum InteractionsByPerspectiveTypes
{
    BothPerspectives,
    Only2D,
    Only3D
}