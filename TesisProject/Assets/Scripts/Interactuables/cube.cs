using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cube : MonoBehaviour, IInteractable, IEscalable,IRotable
{
    [SerializeField] private Vector3 minScale, maxScale;
    [SerializeField] private bool canRotateInY, canRotateInZ;


    public bool CanRotateInY()
    {
        return canRotateInY;
    }

    public bool CanRotateInZ()
    {
        return canRotateInZ;
    }

    public Vector3 GetMaxScale()
    {
        return maxScale;
    }

    public Vector3 GetMinScale()
    {
        return minScale;
    }
}
