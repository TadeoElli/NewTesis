using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    protected GameObject objective;
    protected InputManager inputManager;
    protected CameraManager cameraManager;
    protected MouseState mouseState;
    [SerializeField] protected bool isOn2D;
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected LayerMask interactableLayer;

    public virtual void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        mouseState = GetComponentInParent<MouseState>();
    }
    public virtual void DropInteractable()
    {
        if (objective != null)
        {
            objective = null;
        }
    }
    public virtual void Interact(GameObject interactable, bool isPerspective2D)
    {
        objective = interactable;
        isOn2D = isPerspective2D;
    }
    protected bool IsMouseOverObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Hacemos un raycast para comprobar si el mouse sigue apuntando al objeto interactuable
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableLayer))
        {
            return hit.collider.gameObject == objective;
        }

        return false;
    }
}
