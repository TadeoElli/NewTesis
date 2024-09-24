using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : MonoBehaviour
{
    protected GameObject objective;
    protected PlayerController playerController;
    [SerializeField] protected bool isOn2D;

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
}
