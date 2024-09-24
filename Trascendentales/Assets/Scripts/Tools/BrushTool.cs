using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : Tools
{
    private IPaintable currentInteractable;
    [SerializeField]private float holdTimeThreshold = 1f; // Tiempo necesario para activar la interacción
    [SerializeField]private bool isHolding = false;
    private float holdTime = 0f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        if(!objective.TryGetComponent<IPaintable>(out IPaintable interactuable))
            return;
        playerController.OnPerspectiveSwitch += DropInteractable;
        playerController.OnToolDesinteract += DropInteractable;
        playerController.OnToolSwitchCheck += DropInteractable;
        currentInteractable = interactuable;
        base.Interact(objective, isPerspective2D);

        holdTime = 0f;
        isHolding = true;
    }

    void Update()
    {
        if (isHolding)
        {
            holdTime += Time.deltaTime;

            // Si se cumple el tiempo de hold, se activa la interacción
            if (holdTime >= holdTimeThreshold)
            {
                currentInteractable?.InteractWithBrush(isOn2D);
                isHolding = false; // Detener el hold una vez que se activa
            }
        }
    }

    public override void DropInteractable()
    {
        base.DropInteractable();
        playerController.OnPerspectiveSwitch -= DropInteractable;
        playerController.OnToolSwitchCheck -= DropInteractable;
        playerController.OnToolDesinteract -= DropInteractable;
        if (currentInteractable != null)
        {
            currentInteractable.DropWithBrush(isOn2D);
            currentInteractable = null;
        }
        isHolding = false;
    }
}
