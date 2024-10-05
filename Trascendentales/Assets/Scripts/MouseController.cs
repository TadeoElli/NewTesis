using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Components")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private MouseState mouseState;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Transform cursorTransform;  //transform del cursor
    [SerializeField] public Material outline;

    [Header("Cursor")]
    [SerializeField] private LayerMask interactableLayer;   //layer de todos los objetos interactuables
    [SerializeField] private Sprite brushSprite, rulerSprite, squadSprite, compassSprite, eraserSprite;
    [SerializeField] private float planeOffset;

    [Header("LeftClickTools")]
    [SerializeField] private Tools brushTool;
    [SerializeField] private Tools rulerTool;
    [SerializeField] private Tools squadTool;
    [SerializeField] private Tools compassTool;
    [SerializeField] private Tools eraseTool;

    [Header("RightClickTools")]
    [SerializeField] private Tools drawObjectsTool;
    [SerializeField] private Tools setScaleParentTool;
    [SerializeField] private Tools setRotationParentTool;
    [SerializeField] private Tools dragAtachableObjectsTool;

    private Tools leftClickTool, rightClickTool; // Herramienta activa
    private ToolTypes currentToolType;

    private bool is2DView = false;   // Indica si estamos en la vista 2D o 2.5D
    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        mouseState = GetComponent<MouseState>();
        playerController.OnToolSwitch += SetTool; // Unifica el evento de cambio de herramienta
        playerController.OnPerspectiveSwitch += SwitchView;
        playerController.OnLeftClickPress += PerformRaycastForLeftClick;
        playerController.OnRightClickPress += PerformRaycastForRightClick;
    }

    private void Start()
    {
        Cursor.visible = false; //Desactivo el mouse
        SetTool(ToolTypes.Brush); // Herramienta inicial
    }

    void Update()
    {
        UpdateCursorPosition();   // Actualizar la posición del cursor en pantalla
    }
    // Actualiza la posición del cursor para seguir el mouse
    private void UpdateCursorPosition()
    {
        // Obtener la posición del mouse en el mundo
        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane + planeOffset));

        // En 2.5D (Perspectiva), queremos que el cursor esté en un plano específico (por ejemplo, Z = 0)
        if (is2DView)  // Si la cámara no está en modo ortográfico (está en 2.5D)
        {
            mouseWorldPosition.z = 0f;  // Mantén el cursor en el plano Z = 0
        }

        // Actualiza la posición del transform del cursor para que siga al mouse
        transform.position = mouseWorldPosition;
    }

    // Lanza un Raycast desde la cámara hacia la posición del cursor en el mundo
    private void PerformRaycastForLeftClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            if(!hit.collider.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                Debug.Log("It Was not possible to interact with: " + hit.collider.name);
                return;
            }
            leftClickTool?.Interact(hit.collider.gameObject, is2DView);
            Debug.Log("Interacted with: " + hit.collider.name);
            
        }
    }
    private void PerformRaycastForRightClick()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            rightClickTool?.Interact(hit.collider.gameObject, is2DView);
            Debug.Log("Interacted with: " + hit.collider.name);

        }
    }



    public void SwitchView()
    {
        is2DView = !is2DView;
    }

    private void SetTool(ToolTypes toolType)
    {
        currentToolType = toolType;
        mouseState.SetCurrentToolType(toolType);

        switch (toolType)
        {
            case ToolTypes.Brush:
                spriteRenderer.sprite = brushSprite;
                leftClickTool = brushTool;
                rightClickTool = drawObjectsTool;
                break;
            case ToolTypes.Ruler:
                spriteRenderer.sprite = rulerSprite;
                leftClickTool = rulerTool;
                rightClickTool = setScaleParentTool;
                break;
            case ToolTypes.Squad:
                spriteRenderer.sprite = squadSprite;
                leftClickTool = squadTool;
                rightClickTool = setRotationParentTool;
                break;
            case ToolTypes.Compass:
                spriteRenderer.sprite = compassSprite;
                leftClickTool = compassTool;
                rightClickTool = dragAtachableObjectsTool;
                break;
            case ToolTypes.Eraser:
                spriteRenderer.sprite = eraserSprite;
                leftClickTool = eraseTool;
                break;
            default:
                break;
        }
    }

   
}
