using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InteractuableObject))]
public class RotableObject : MonoBehaviour, IRotable
{
    [SerializeField] private bool canRotateInY;
    [SerializeField] private bool canRotateInZ;
    [SerializeField] private float maxRadius;
    [SerializeField] private bool canAttachOthers;
    [SerializeField] private Transform gimballRef;
    public event Action OnEraserInteract;

    public bool CanRotateInY() => canRotateInY;
    public bool CanRotateInZ() => canRotateInZ;
    public bool CanAttachOthers() => canAttachOthers;
    public float GetMaxRadius() => maxRadius;


    public void InteractWithEraser(bool isOn2D)
    {
        OnEraserInteract?.Invoke();
    }
    public void SetGimballRef(Transform newRef)
    {
        gimballRef = newRef;
    }

    public void SetRotationConstraints(bool canRotateInY, bool canRotateInZ)
    {
        this.canRotateInY = canRotateInY;
        this.canRotateInZ = canRotateInZ;
    }
}
