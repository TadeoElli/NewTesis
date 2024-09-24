using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InteractuableObject))]
public class CompassableObject : MonoBehaviour, ICompassable
{
    [SerializeField]private float maxRadius;

    public event Action OnEraserInteract;
    public event Action OnEraserDrop;
    public event Action OnCompassInteract;

    public float GetMaxRadius() => maxRadius;

    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
        OnEraserInteract = null;
    }
    public void DropWithEraser(bool isOn2D)
    {
        OnEraserDrop?.Invoke();
    }

    public void InteractWithCompass(bool isOn2D)
    {
        OnCompassInteract?.Invoke();
    }

    public void SetMaxRadius(float maxRadius)
    {
        this.maxRadius = maxRadius;
    }
}

