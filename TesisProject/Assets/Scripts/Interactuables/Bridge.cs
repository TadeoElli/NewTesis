using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour, IInteractable , IPaintable
{
    [SerializeField] Vector3 newPosition;

    public void DropWithBrush(bool isOn2D)
    {

    }

    public void InteractWithBrush(bool isIn2D)
    {
        if (isIn2D)
        {
            transform.position = newPosition;
        }
    }
}
