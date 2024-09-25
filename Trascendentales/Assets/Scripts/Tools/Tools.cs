using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    protected GameObject objective;
    protected PlayerController playerController;
    [SerializeField] protected bool isOn2D;
    [SerializeField] protected Camera mainCamera;
    [SerializeField] protected LayerMask interactableLayer;

    public virtual void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
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
