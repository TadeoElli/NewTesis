using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushTool : Tools
{
    private IPaintable currentInteractable;
    [SerializeField] private GameObject chargeCursor;
    [SerializeField] private Image image;
    [SerializeField]private float holdTimeThreshold = 1f; // Tiempo necesario para activar la interacción
    [SerializeField]private bool isHolding = false;
    private float holdTime = 0f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        base.Interact(objective, isPerspective2D);
        if(!objective.TryGetComponent<IPaintable>(out IPaintable interactuable))
            return;
        currentInteractable = interactuable;
        if(!currentInteractable.CanInteractWithBrush(isOn2D))
            return;
        playerController.OnPerspectiveSwitch += DropInteractable;
        playerController.OnToolDesinteract += DropInteractable;
        playerController.OnToolSwitchCheck += DropInteractable;
        chargeCursor.SetActive(true);
        image.fillAmount = 0f; // Reiniciar el fillAmount a 0
        holdTime = 0f;
        
        isHolding = true;
    }

    void Update()
    {

        if (isHolding)
        {
            holdTime += Time.deltaTime;
            image.fillAmount = holdTime / holdTimeThreshold;
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
        chargeCursor.SetActive(false);
        image.fillAmount = 0f;
        if (currentInteractable != null)
        {
            currentInteractable = null;
        }
        isHolding = false;
    }
}
