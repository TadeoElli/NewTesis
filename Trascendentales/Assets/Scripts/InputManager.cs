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

    //Events
    public event Action<ToolTypes> OnToolSwitch;
    public event Action OnToolSwitchCheck;
    public event Action OnPerspectiveSwitch;
    public event Action OnLeftClickPress;
    public event Action OnLeftClickDrop;
    public event Action OnRightClickPress;
    public event Action OnRightClickDrop;
    public event Action OnChangeCameraToLeft;
    public event Action OnChangeCameraToRight;

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
            playerControls.PlayerCamera.TurnCameraToLeft.performed += _ => SwitchCameraToLeft();
            playerControls.PlayerCamera.TurnCameraToRight.performed += _ => SwitchCameraToRight();
            playerControls.PlayerCamera.SwitchCamera.performed += _ => SwitchCamera();
            playerControls.PlayerActions.Jump.performed += _ => jump_input = true;
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
    private void SwitchCameraToLeft()
    {
        OnChangeCameraToLeft?.Invoke();
    }
    private void SwitchCameraToRight()
    {
        OnChangeCameraToRight?.Invoke();
    }
    private void SwitchCamera()
    {
        OnPerspectiveSwitch?.Invoke();
    }
}
