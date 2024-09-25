using System;
using UnityEngine;

public interface IInteractable
{
    public bool IsAtachable();
    public bool IsAtached();
    public void SetIsAtached(bool newAtached);
}

public interface IEscalable
{
    public Vector3 GetMinScale();
    public Vector3 GetMaxScale();
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
