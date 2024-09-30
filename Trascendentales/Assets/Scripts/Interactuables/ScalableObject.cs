using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InteractuableObject))]
public class ScalableObject : MonoBehaviour, IEscalable
{
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float maxRadius;
    [SerializeField] private bool canAttachOthers;
    public event Action OnEraserInteract;

    public Vector3 GetMinScale() => minScale;
    public Vector3 GetMaxScale() => maxScale;

    public void SetScaleConstraints(Vector3 minScale, Vector3 maxScale)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;
    }

    public bool CanAttachOthers()
    {
        return canAttachOthers;
    }

    public float GetMaxRadius()
    {
        return maxRadius;
    }

    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }
}
