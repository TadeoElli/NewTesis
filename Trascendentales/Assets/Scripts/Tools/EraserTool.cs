using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EraserTool : Tools
{
    private IPaintable currentPaintable;
    private ICompassable currentCompassable;
    [SerializeField] private float holdTimeThreshold = 1f; // Tiempo necesario para activar la interacción
    [SerializeField] private bool isHolding = false;
    private float holdTime = 0f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        if (objective.TryGetComponent<IPaintable>(out IPaintable paintable))
        {
            currentPaintable = paintable;
        }
        if (objective.TryGetComponent<ICompassable>(out ICompassable compassable))
        {
            currentCompassable = compassable;
        }
        if(currentPaintable ==  null && currentCompassable == null)
            return;
        playerController.OnPerspectiveSwitch += DropInteractable;
        playerController.OnToolDesinteract += DropInteractable;
        playerController.OnToolSwitchCheck += DropInteractable;
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
                currentCompassable?.InteractWithEraser(isOn2D);
                currentPaintable?.InteractWithEraser(isOn2D);
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
        if (currentCompassable != null)
        {
            currentCompassable.DropWithEraser(isOn2D);
            currentCompassable = null;
        }
        if (currentPaintable != null)
        {
            currentPaintable.DropWithEraser(isOn2D);
            currentPaintable = null;
        }
        isHolding = false;
    }
}
