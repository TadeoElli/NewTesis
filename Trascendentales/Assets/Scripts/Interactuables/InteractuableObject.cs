using UnityEngine;

public class InteractuableObject : MonoBehaviour, IInteractable
{
    [SerializeField] protected bool isAtacheable;
    [SerializeField] private bool isAtached = false;
    private GameObject parent = null;
    public bool IsAtachable() => isAtacheable;
    public bool IsAtached() => isAtached;

    public void SetIsAtached(GameObject gameObject)
    {
        isAtached = true;
        parent = gameObject;
    }
    public void SetUnatached()
    {
        isAtached = false;
        parent = null;
    }
    public GameObject GetParent()
    {
        return parent;
    }

}
