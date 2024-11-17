using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    // Tiempo de cooldown en segundos para cambios de cámara y ángulo
    [SerializeField] private float cameraSwitchCooldown = 1f;
    [SerializeField] private float angleSwitchCooldown = 1f;

    // Estados para los cooldowns
    private bool canSwitchCamera = true;
    private bool canSwitchAngle = true;

    PlayerControls playerControls;
    PlayerLocomotion playerLocomotion;
    PlayerManager playerManager;
    AnimatorManager animatorManager;
    MenuManager menuManager;
    Vector2 movementInput;
    private float moveAmount;
    [HideInInspector] public float verticalInput;
    [HideInInspector] public float horizontalInput;
    [HideInInspector] public bool jump_input;
    [HideInInspector] public bool isOn2D = false;
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
    public event Action OnInteract;

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
            playerControls.PlayerActions.OpenAlternativeWheel.performed += _ => ShowAlternativeWheel();
            playerControls.PlayerActions.OpenAlternativeWheel.canceled += _ => HideAlternativeWheel();
            playerControls.PlayerActions.LeftClick.performed += _ => LeftClickPressed();
            playerControls.PlayerActions.LeftClick.canceled += _ => LeftClickReleased();
            playerControls.PlayerActions.RightClick.performed += _ => RightClickPressed();
            playerControls.PlayerActions.RightClick.canceled += _ => RightClickReleased();
            playerControls.PlayerActions.Escape.performed += _ => TogglePause();
            playerControls.PlayerActions.Crouch.performed += _ => GrabInteraction();
            playerControls.PlayerActions.Interact.performed += _ => Interact();
        }
        playerControls.Enable();
    }

    public void OnDisable()
    {
        if(playerControls != null)
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
        if (canSwitchAngle)
        {
            OnChangeCameraAngle?.Invoke();
            StartCoroutine(CameraAngleCooldown());
        }
    }
    private void SwitchCamera()
    {
        if (canSwitchCamera)
        {
            isOn2D = !isOn2D;
            if (isOn2D)
                playerLocomotion.SwitchTo2D();
            else
                playerLocomotion.SwitchTo3D();

            Debug.Log("Perspective switch");
            OnPerspectiveSwitch?.Invoke();
            StartCoroutine(CameraSwitchCooldown());
        }
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
    public void ShowAlternativeWheel()
    {
        switch (MouseState.Instance.CurrentToolActive())
        {
            case ToolTypes.Brush:
                menuManager.ShowDrawObjectWheel();
                break;
            case ToolTypes.Ruler:
                break;
            case ToolTypes.Squad:
                break;
            case ToolTypes.Compass:
                menuManager.ShowDragObjectWheel();
                break;
            case ToolTypes.Eraser:
                menuManager.ShowEraserWheel();
                break;
            default:
                break;
        }
    }
    public void HideAlternativeWheel()
    {
        menuManager.HideAlternativeWheel();
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

    public void Interact()
    {
        OnInteract?.Invoke();
    }
    #region Cooldown Coroutines
    private IEnumerator CameraAngleCooldown()
    {
        canSwitchAngle = false;
        yield return new WaitForSeconds(angleSwitchCooldown);
        canSwitchAngle = true;
    }

    private IEnumerator CameraSwitchCooldown()
    {
        canSwitchCamera = false;
        yield return new WaitForSeconds(cameraSwitchCooldown);
        canSwitchCamera = true;
    }
        #endregion
}