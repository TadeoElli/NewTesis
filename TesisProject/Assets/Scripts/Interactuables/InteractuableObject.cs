using UnityEngine;

public class InteractuableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool isAtacheable;
    [SerializeField] private bool isAtached = false;
    public bool IsAtachable() => isAtacheable;
    public bool IsAtached() => isAtached;

    public void SetIsAtached(bool newAtached)
    {
        this.isAtached = newAtached;
    }
}
