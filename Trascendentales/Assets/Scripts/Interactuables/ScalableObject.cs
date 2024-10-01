using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(InteractuableObject))]
public class ScalableObject : MonoBehaviour, IEscalable
{
    [SerializeField] private Vector3 minScale;
    [SerializeField] private Vector3 maxScale;
    [SerializeField] private float maxRadius;
    [SerializeField] private bool canAttachOthers;
    [SerializeField] public bool canScale = true;
    public event Action OnEraserInteract;

    public Vector3 GetMinScale() => minScale;
    public Vector3 GetMaxScale() => maxScale;
    public bool CanAttachOthers() => canAttachOthers;
    public bool CanScale() => canScale;
    public float GetMaxRadius() => maxRadius;

    public void SetScaleConstraints(Vector3 minScale, Vector3 maxScale)
    {
        this.minScale = minScale;
        this.maxScale = maxScale;
    }

    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }

}
