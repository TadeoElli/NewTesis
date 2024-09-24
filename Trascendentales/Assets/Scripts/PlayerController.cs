using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event Action<ToolTypes> OnToolSwitch;
    public event Action OnToolSwitchCheck;
    public event Action OnPerspectiveSwitch;
    public event Action OnToolInteract;
    public event Action OnToolDesinteract;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnToolSwitch?.Invoke(ToolTypes.Brush);
            OnToolSwitchCheck?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnToolSwitch?.Invoke(ToolTypes.Ruler);
            OnToolSwitchCheck?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnToolSwitch?.Invoke(ToolTypes.Squad);
            OnToolSwitchCheck?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnToolSwitch?.Invoke(ToolTypes.Compass);
            OnToolSwitchCheck?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnToolSwitch?.Invoke(ToolTypes.Eraser);
            OnToolSwitchCheck?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
            OnPerspectiveSwitch?.Invoke();
        if (Input.GetMouseButtonDown(0))
            OnToolInteract?.Invoke();
        if (Input.GetMouseButtonUp(0))
            OnToolDesinteract?.Invoke();
    }
}
