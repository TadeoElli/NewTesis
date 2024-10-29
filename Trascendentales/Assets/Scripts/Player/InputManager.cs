using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls, playerControlsOnLive;
    PlayerLocomotion playerLocomotion;
    AnimatorManager animatorManager;
    Vector2 movementInput;
    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    public bool jump_input;
    public bool isOn2D = false;
    [SerializeField] private Animator toolWheelAnim, drawObjectWheelAnim, playerAnim;
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
        animatorManager = GetComponent<AnimatorManager>();
    }
    private void OnEnable()
    {
        if (playerControls == null && playerControlsOnLive == null)
        {
            playerControlsOnLive = new PlayerControls();
            playerControls = new PlayerControls();

            playerControlsOnLive.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControlsOnLive.PlayerCamera.SwitchCameraAngle.performed += _ => SwitchCameraAngle();
            playerControlsOnLive.PlayerCamera.SwitchCamera.performed += _ => SwitchCamera();
            playerControlsOnLive.PlayerActions.Jump.performed += _ => jump_input = true;
            playerControlsOnLive.PlayerActions.Tool1.performed += _ => SetBrushTool();
            playerControlsOnLive.PlayerActions.Tool2.performed += _ => SetRulerTool();
            playerControlsOnLive.PlayerActions.Tool3.performed += _ => SetSquadTool();
            playerControlsOnLive.PlayerActions.Tool4.performed += _ => SetCompassTool();
            playerControlsOnLive.PlayerActions.Tool5.performed += _ => SetEraserTool();
            playerControlsOnLive.PlayerActions.OpenToolWheel.performed += _ => ShowToolWheel();
            playerControlsOnLive.PlayerActions.OpenToolWheel.canceled += _ => HideToolWheel();
            playerControlsOnLive.PlayerActions.LeftClick.performed += _ => LeftClickPressed();
            playerControlsOnLive.PlayerActions.LeftClick.canceled += _ => LeftClickReleased();
            playerControlsOnLive.PlayerActions.RightClick.performed += _ => RightClickPressed();
            playerControlsOnLive.PlayerActions.RightClick.canceled += _ => RightClickReleased();
            playerControls.PlayerActions.Escape.performed += _ => TogglePause();
            playerControls.PlayerActions.Death.performed += _ => Death();
        }
        playerControls.Enable();
        playerControlsOnLive.Enable();
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
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        animatorManager.UpdateAnimatorValues(0, moveAmount);
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
    public void SetBrushTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Brush);
        OnToolSwitchCheck?.Invoke();
    }
    public void SetRulerTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Ruler);
        OnToolSwitchCheck?.Invoke();
    }
    public void SetSquadTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Squad);
        OnToolSwitchCheck?.Invoke();
    }
    public void SetCompassTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Compass);
        OnToolSwitchCheck?.Invoke();
    }
    public void SetEraserTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Eraser);
        OnToolSwitchCheck?.Invoke();
    }
    public void ShowToolWheel()
    {
        toolWheelAnim.SetBool("OpenToolWheel", true);
        OnToolSwitchCheck?.Invoke();
    }
    public void HideToolWheel()
    {
        toolWheelAnim.SetBool("OpenToolWheel", false);
    }
    public void ShowDrawObjectWheel()
    {
        drawObjectWheelAnim.SetBool("OpenToolWheel", true);
        OnToolSwitchCheck?.Invoke();
    }
    public void HideDrawObjectWheel()
    {
        drawObjectWheelAnim.SetBool("OpenToolWheel", false);
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

    public void Death()
    {
        movementInput = Vector2.zero;
        GetComponent<Rigidbody>().velocity = Vector2.zero;
        HandleMovementInput();
        playerControlsOnLive.Disable();
    }
}
