using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EraserTool : Tools
{
    private IPaintable currentPaintable;
    private ICompassable currentCompassable;
    private IEscalable currentEscalable;
    private IRotable currentRotable;
    
    [SerializeField] private GameObject chargeCursor;
    [SerializeField] private Image image;
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
            if (!currentPaintable.CanInteractWithEraser(isOn2D))
                return;
        }
        if (objective.TryGetComponent<ICompassable>(out ICompassable compassable))
        {
            currentCompassable = compassable;
        }
        if (objective.TryGetComponent<IEscalable>(out IEscalable escalable))
        {
            currentEscalable = escalable;
        }
        if (objective.TryGetComponent<IRotable>(out IRotable rotable))
        {
            currentRotable = rotable;
        }
        if (currentPaintable ==  null && currentCompassable == null && currentEscalable == null && currentRotable == null)
            return;
        mouseState.SetLeftclickPress();
        inputManager.OnPerspectiveSwitch += DropInteractable;
        inputManager.OnLeftClickDrop += DropInteractable;
        inputManager.OnToolSwitchCheck += DropInteractable;
        chargeCursor.SetActive(true);
        image.fillAmount = 0f; // Reiniciar el fillAmount a 0
        base.Interact(objective, isPerspective2D);

        holdTime = 0f;
        isHolding = true;
    }

    void Update()
    {

        
        if (isHolding)
        {
            if (!IsMouseOverObject())
            {
                // Si el mouse deja de apuntar al objeto, se reinicia el progreso
                isHolding = false;
                holdTime = 0f;
                image.fillAmount = 0f;
                chargeCursor.SetActive(false);
                return;
            }
            holdTime += Time.deltaTime;
            image.fillAmount = holdTime / holdTimeThreshold;
            // Si se cumple el tiempo de hold, se activa la interacción
            if (holdTime >= holdTimeThreshold)
            {
                currentCompassable?.InteractWithEraser(isOn2D);
                currentPaintable?.InteractWithEraser(isOn2D);
                currentEscalable?.InteractWithEraser(isOn2D);
                currentRotable?.InteractWithEraser(isOn2D);
                isHolding = false; // Detener el hold una vez que se activa
            }
        }
    }

    public override void DropInteractable()
    {
        base.DropInteractable();
        mouseState.DropLeftClick();
        inputManager.OnPerspectiveSwitch -= DropInteractable;
        inputManager.OnToolSwitchCheck -= DropInteractable;
        inputManager.OnLeftClickDrop -= DropInteractable;
        chargeCursor.SetActive(false);
        image.fillAmount = 0f;
        if (currentCompassable != null)
        {
            currentCompassable.DropWithEraser(isOn2D);
            currentCompassable = null;
        }
        if (currentPaintable != null)
        {
            currentPaintable = null;
        }
        if (currentEscalable != null)
        {
            currentEscalable = null;
        }
        if (currentRotable != null)
        {
            currentRotable = null;
        }
        isHolding = false;
    }
}
