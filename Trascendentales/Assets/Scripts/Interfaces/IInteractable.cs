using System;
using UnityEngine;

public interface IInteractable
{
    public bool IsAtachableForCompass();
    public bool IsAtachableForRuler();
    public bool IsAtachableForSquad();
    public bool IsAtachedToCompass();
    public void SetIsAtachedToCompass(GameObject parent);
    public void SetUnatachedToCompass();
    public void SetIsAtachableForRuler(bool isAtachable);
    public GameObject GetCompassParent();
}

public interface IEscalable
{
    public Vector3 GetMinScale();
    public Vector3 GetMaxScale();
    public bool CanAttachOthers();
    public bool CanScale();
    public float GetMaxRadius();
    public void InteractWithEraser(bool isOn2D);
    public event Action OnEraserInteract;
}
public interface IRotable
{
    public bool CanRotateInY();
    public bool CanRotateInZ();
    public bool CanAttachOthers();
    public float GetMaxRadius();
    public void SetGimballRef(Transform newRef);
    public void InteractWithEraser(bool isOn2D);
    public event Action OnEraserInteract;
}
public interface IPaintable
{
    public void InteractWithBrush(bool isOn2D);
    public void InteractWithEraser(bool isOn2D);
    public bool CanInteractWithBrush(bool isOn2D);
    public bool CanInteractWithEraser(bool isOn2D);
}
public interface ICompassable
{
    public float GetMaxRadius();
    public void InteractWithCompass(bool isOn2D);
    public void InteractWithEraser(bool isOn2D);
    public void DropWithEraser(bool isOn2D);
    public event Action OnEraserInteract;

}
