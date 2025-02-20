using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    // Tiempo de cooldown en segundos para cambios de cámara y ángulo
    [SerializeField] private float cameraSwitchCooldown = 1f;
    [SerializeField] private float angleSwitchCooldown = 1f;

    // Estados para los cooldowns
    private bool canSwitchCamera = true;
    private bool canSwitchAngle = true;

    PlayerControls playerActions, playerControls;
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

    public VisualEffect _shieldVfx;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject); // Asegurarse de que solo haya una instancia

        _shieldVfx.Stop();
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
        if (playerActions == null)
        {
            playerActions = new PlayerControls();

            playerActions.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();

            playerActions.PlayerCamera.SwitchCameraAngle.performed += _ => SwitchCameraAngle();
            playerActions.PlayerCamera.SwitchCamera.performed += _ => SwitchCamera();
            playerActions.PlayerActions.Jump.performed += _ => jump_input = true;
            playerActions.PlayerActions.Tool1.performed += _ => SetBrushTool();
            playerActions.PlayerActions.Tool2.performed += _ => SetRulerTool();
            playerActions.PlayerActions.Tool3.performed += _ => SetSquadTool();
            playerActions.PlayerActions.Tool4.performed += _ => SetCompassTool();
            playerActions.PlayerActions.Tool5.performed += _ => SetEraserTool();
            playerActions.PlayerActions.OpenToolWheel.performed += _ => ShowToolWheel();
            playerActions.PlayerActions.OpenToolWheel.canceled += _ => HideToolWheel();
            playerActions.PlayerActions.OpenAlternativeWheel.performed += _ => ShowAlternativeWheel();
            playerActions.PlayerActions.OpenAlternativeWheel.canceled += _ => HideAlternativeWheel();
            playerActions.PlayerActions.LeftClick.performed += _ => LeftClickPressed();
            playerActions.PlayerActions.LeftClick.canceled += _ => LeftClickReleased();
            playerActions.PlayerActions.RightClick.performed += _ => RightClickPressed();
            playerActions.PlayerActions.RightClick.canceled += _ => RightClickReleased();
            playerActions.PlayerActions.Crouch.performed += _ => GrabInteraction();
        }
        playerActions.Enable();
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerInteractions.Interact.performed += _ => Interact();
            playerControls.PlayerInteractions.Escape.performed += _ => TogglePause();
        }
        playerControls.Enable();
    }

    public void OnDisable()
    {
        if(playerControls != null)
            playerActions.Disable();
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
        playerActions.Disable();
        playerActions = null;
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