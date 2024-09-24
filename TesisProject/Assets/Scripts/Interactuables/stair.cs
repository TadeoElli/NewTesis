using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stair : MonoBehaviour, IInteractable,IRotable
{
    [SerializeField] private bool canRotateInY, canRotateInZ;
    public virtual bool CanRotateInY()
    {
        return canRotateInY;
    }

    public bool CanRotateInZ()
    {
        return canRotateInZ;
    }

    public bool IsAtachable()
    {
        return false;
    }
}
