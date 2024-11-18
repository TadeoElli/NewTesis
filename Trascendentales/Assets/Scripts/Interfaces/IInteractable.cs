using System;
using UnityEngine;

public interface IInteractable
{
    public bool IsAtachableForCompass();
    public bool IsRotatingTowardsTheCompass();
    public bool IsAtachableForRuler();
    public bool IsAtachableForSquad();
    public bool IsAtachedToCompass();
    public bool IsAtachedToRuler();
    public bool IsAtachedToSquad();
    public void SetIsAtachedToCompass(GameObject parent);
    public void SetIsAtachedToRuler();
    public void SetIsAtachedToSquad();
    public void SetUnatachedToCompass();
    public void SetUnatachedToRuler();
    public void SetUnatachedToSquad();
    public void SetIsAtachableForRuler(bool isAtachable);
    public GameObject GetCompassParent();
    public void InteractWithEraser(bool isOn2D);
    public event Action OnEraserInteract;
}

public interface IEscalable
{
    public Vector3 GetMinScale();
    public Vector3 GetMaxScale();
    public bool CanAttachOthers();
    public bool CanScale();
    public bool IsScaling();
    public void SetIsScaling(bool state);
    public float GetMaxRadius();
    public void InteractWithEraser(bool isOn2D);
    public event Action OnEraserInteract;
}
public interface IRotable
{
    public bool CanRotate();
    public bool IsRotating();
    public void SetIsRotating(bool state);
    public bool NeedToBeKinematic();
    public bool CanRotateInY();
    public bool CanRotateInZ();
    public bool CanAttachOthers();
    public float GetMaxRadius();
    public void SetGimballRef(Transform newRef);
    public void SetCanRotate(bool canRotate);
    public void InteractWithEraser(bool isOn2D);
    public event Action OnEraserInteract;
}
public interface IRotableClamp
{
    public bool IsClamped();
    public float GetMinRotationY();
    public float GetMaxRotationY();
    public float GetMinRotationZ();
    public float GetMaxRotationZ();
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
    public void InteractWithEraser(bool isOn2D);
    public void DropWithEraser(bool isOn2D);
    public event Action OnEraserInteract;

}
public interface IMovable
{
    public float GetMaxRadius();
    public bool GetNeedGravity();
    public bool GetIsMovable();
    public void ShowOriginFeedback();
    public Vector3 GetOriginalPosition();
    public event Action OnEraserInteract;
    public void InteractWithEraser(bool isOn2D);

}
public interface IIlluminable
{
    public void OnLightOn();
}


