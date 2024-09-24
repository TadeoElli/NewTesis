using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cillinder : MonoBehaviour, IInteractable,IRotable, ICompassable
{
    [SerializeField] private bool canRotateInY, canRotateInZ;
    [SerializeField] private float maxRadius;
    public bool CanRotateInY()
    {
        return canRotateInY;
    }

    public bool CanRotateInZ()
    {
        return canRotateInZ;
    }

    public float GetMaxRadius()
    {
        return maxRadius;
    }

    public void InteractWithCompass(bool isOn2D)
    {
        //throw new System.NotImplementedException();
    }

    public bool IsAtachable()
    {
        return true;
    }
}
