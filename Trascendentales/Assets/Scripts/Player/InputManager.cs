using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    MenuManager menuManager;
    Vector2 movementInput;
    private float moveAmount;
    public float verticalInput;
    public float horizontalInput;
    public bool jump_input;
    public bool isOn2D = false;
    private bool isGrabbing = false;
    private GameObject platform;
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
        playerManager = GetComponent<PlayerManager>();
    }
    private void Start()
    {
        menuManager = MenuManager.Instance;
    }
    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControls.PlayerCamera.SwitchCameraAngle.performed += _ => SwitchCameraAngle();
            playerControls.PlayerCamera.SwitchCamera.performed += _ => SwitchCamera();
            playerControls.PlayerActions.Jump.performed += _ => jump_input = true;
            playerControls.PlayerActions.Tool1.performed += _ => SetBrushTool();
            playerControls.PlayerActions.Tool2.performed += _ => SetRulerTool();
            playerControls.PlayerActions.Tool3.performed += _ => SetSquadTool();
            playerControls.PlayerActions.Tool4.performed += _ => SetCompassTool();
            playerControls.PlayerActions.Tool5.performed += _ => SetEraserTool();
            playerControls.PlayerActions.OpenToolWheel.performed += _ => ShowToolWheel();
            playerControls.PlayerActions.OpenToolWheel.canceled += _ => HideToolWheel();
            playerControls.PlayerActions.LeftClick.performed += _ => LeftClickPressed();
            playerControls.PlayerActions.LeftClick.canceled += _ => LeftClickReleased();
            playerControls.PlayerActions.RightClick.performed += _ => RightClickPressed();
            playerControls.PlayerActions.RightClick.canceled += _ => RightClickReleased();
            playerControls.PlayerActions.Escape.performed += _ => TogglePause();
            playerControls.PlayerActions.Crouch.performed += _ => GrabInteraction();
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
        //HideDrawObjectWheel();
    }
    public void SetSquadTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Squad);
        OnToolSwitchCheck?.Invoke();
        //HideDrawObjectWheel();
    }
    public void SetCompassTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Compass);
        OnToolSwitchCheck?.Invoke();
        //HideDrawObjectWheel();
    }
    public void SetEraserTool()
    {
        OnToolSwitch?.Invoke(ToolTypes.Eraser);
        OnToolSwitchCheck?.Invoke();
        //HideDrawObjectWheel();
    }
    public void ShowToolWheel()
    {
        menuManager.ShowToolWheel();
        OnToolSwitchCheck?.Invoke();
    }
    public void HideToolWheel()
    {
        menuManager.HideToolWheel();
    }
    public void ShowDrawObjectWheel()
    {
        menuManager.ShowDrawObjectWheel();
    }
    public void HideDrawObjectWheel()
    {
        menuManager.HideDrawObjectWheel();
    }
    public void ShowDragObjectWheel()
    {
        menuManager.ShowDragObjectWheel();
    }
    public void HideDragObjectWheel()
    {
        menuManager.HideDragObjectWheel();
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
        MenuManager.Instance.TogglePause();
    }
    #endregion

    public void Death()
    {
        movementInput = Vector2.zero;
        GetComponent<Rigidbody>().velocity = Vector2.zero;
        HandleMovementInput();
        playerControls.Disable();
        playerControls = null;
    }
    public void GrabInteraction()
    {
        if (isGrabbing)
        {
            // Si está agarrado a algo, intenta soltarlo
            playerLocomotion.TryToDrop();
            isGrabbing = false;
        }
        else
        {
            // Si no está agarrado, intenta agarrar
            isGrabbing = playerLocomotion.TryToGrab();
        }
    }

}
