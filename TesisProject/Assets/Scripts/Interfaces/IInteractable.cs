using UnityEngine;

public interface IInteractable
{
    public bool IsAtachable();
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
    public void DropWithBrush(bool isOn2D);
}
public interface ICompassable
{
    public float GetMaxRadius();
    public void InteractWithCompass(bool isOn2D);

}
