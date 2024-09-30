using System;
using UnityEngine;

public interface IInteractable
{
    public bool IsAtachableForCompass();
    public bool IsAtachableForRuler();
    public bool IsAtachedToCompass();
    public bool IsAtachedToRuler();
    public void SetIsAtachedToCompass(GameObject parent);
    public void SetIsAtachedToRuler(GameObject parent);
    public void SetUnatachedToCompass();
    public void SetUnatachedToRuler();
    public GameObject GetCompassParent();
    public GameObject GetRulerParent();
}

public interface IEscalable
{
    public Vector3 GetMinScale();
    public Vector3 GetMaxScale();
    public bool CanAttachOthers();
    public float GetMaxRadius();
    public void InteractWithEraser(bool isOn2D);
    public event Action OnEraserInteract;
}
public interface IRotable
{
    public bool CanRotateInY();
    public bool CanRotateInZ();
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
