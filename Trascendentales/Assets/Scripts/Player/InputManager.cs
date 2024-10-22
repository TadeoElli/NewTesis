using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    Vector2 movementInput;
    public float verticalInput;
    public float horizontalInput;
    public bool jump_input;
    public bool isOn2D = false;
    //Events
    public event Action<ToolTypes> OnToolSwitch;
    public event Action OnToolSwitchCheck;
    public event Action OnPerspectiveSwitch;
    public event Action OnLeftClickPress;
    public event Action OnLeftClickDrop;
    public event Action OnRightClickPress;
    public event Action OnRightClickDrop;
    public event Action OnChangeCameraAngle;

    private void Awake()
    {
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            // Vincular las acciones de las teclas Q y E
            playerControls.PlayerCamera.SwitchCameraAngle.performed += _ => SwitchCameraAngle();
            playerControls.PlayerCamera.SwitchCamera.performed += _ => SwitchCamera();
            playerControls.PlayerActions.Jump.performed += _ => jump_input = true;
            playerControls.PlayerActions.Tool1.performed += _ => SetBrushTool();
            playerControls.PlayerActions.Tool2.performed += _ => SetRulerTool();
            playerControls.PlayerActions.Tool3.performed += _ => SetSquadTool();
            playerControls.PlayerActions.Tool4.performed += _ => SetCompassTool();
            playerControls.PlayerActions.Tool5.performed += _ => SetEraserTool();
            playerControls.PlayerActions.LeftClick.performed += _ => LeftClickPressed();
            playerControls.PlayerActions.LeftClick.canceled += _ => LeftClickReleased();
            playerControls.PlayerActions.RightClick.performed += _ => RightClickPressed();
            playerControls.PlayerActions.RightClick.canceled += _ => RightClickReleased();
            playerControls.PlayerActions.Escape.performed += _ => TogglePause();
        }
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleJumpingInput();
    }
    #region Movement

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }
    private void HandleJumpingInput()
    {
        if(jump_input)
        {
            jump_input = false;
            playerLocomotion.BufferJump();
        }
    }

    #endregion
    #region Camera

    private void SwitchCameraAngle()
    {
        OnChangeCameraAngle?.Invoke();
    }
    private void SwitchCamera()
    {
        isOn2D = !isOn2D;
        if(isOn2D)
            playerLocomotion.SwitchTo2D();
        else
            playerLocomotion.SwitchTo3D();
        Debug.Log("Perspectiveswitch");
        OnPerspectiveSwitch?.Invoke();

    }

    #endregion

    #region Tools
    private void SetBrushTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Brush);
        OnToolSwitchCheck?.Invoke();
    }
    private void SetRulerTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Ruler);
        OnToolSwitchCheck?.Invoke();
    }
    private void SetSquadTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Squad);
        OnToolSwitchCheck?.Invoke();
    }
    private void SetCompassTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Compass);
        OnToolSwitchCheck?.Invoke();
    }
    private void SetEraserTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Eraser);
        OnToolSwitchCheck?.Invoke();
    }
    #endregion
    #region MouseClick
    private void LeftClickPressed()
    {
        //Debug.Log("Left Click Pressed");
        OnLeftClickPress?.Invoke();
    }
    private void LeftClickReleased()
    {
        //Debug.Log("Left Click Released");
        OnLeftClickDrop?.Invoke();
    }
    private void RightClickPressed()
    {
        //Debug.Log("Right Click Pressed");
        OnRightClickPress?.Invoke();
    }
    private void RightClickReleased()
    {
        //Debug.Log("Right Click Released");
        OnRightClickDrop?.Invoke();
    }
    #endregion

    #region Menu
    public void TogglePause()
    {
        GameManager.Instance.TogglePause();
    }
    #endregion
}
