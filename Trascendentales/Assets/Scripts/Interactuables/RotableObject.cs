using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(InteractuableObject))]
public class RotableObject : MonoBehaviour, IRotable
{
    [SerializeField] private bool canRotateInY;
    [SerializeField] private bool canRotateInZ;
    

    public bool CanRotateInY() => canRotateInY;
    public bool CanRotateInZ() => canRotateInZ;


    public void SetRotationConstraints(bool canRotateInY, bool canRotateInZ)
    {
        this.canRotateInY = canRotateInY;
        this.canRotateInZ = canRotateInZ;
    }
}
