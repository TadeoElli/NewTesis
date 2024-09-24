using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadTool : Tools
{
    private Rigidbody objectiveRB;
    private Transform objectiveTr;
    [SerializeField] private Transform gimball;
    [Header("Parameters")]
    [SerializeField] private float rotationSpeed = 1f;  // Sensibilidad de rotación
    private Vector3 lastMousePosition;                  // Última posición del mouse para calcular el movimiento
    private bool canRotateInY, canRotateInZ;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject interactable, bool isPerspective2D)
    {
        if(!interactable.TryGetComponent<IRotable>(out IRotable component))
            return;
        playerController.OnPerspectiveSwitch += DropInteractable;
        playerController.OnToolDesinteract += DropInteractable;
        canRotateInY = component.CanRotateInY();
        canRotateInZ = component.CanRotateInZ();
        base.Interact(interactable, isPerspective2D); // Llama a la lógica común de interactuar
        objectiveRB = objective.GetComponent<Rigidbody>();
        objectiveRB.isKinematic = true;
        objectiveTr = objective.GetComponent<Transform>();
        gimball.position = objectiveTr.position;
        objectiveTr.SetParent(gimball);
        lastMousePosition = Input.mousePosition;  // Guarda la posición actual del mouse al iniciar la interacción
    }
    public override void DropInteractable()
    {
        if(objective == null) return;
        objectiveRB.isKinematic = false;
        objectiveRB = null;
        objectiveTr.SetParent(null);
        objectiveTr = null;
        ResetGimball();
        base.DropInteractable();
        playerController.OnPerspectiveSwitch -= DropInteractable;
        playerController.OnToolDesinteract -= DropInteractable;
    }

    private void ResetGimball()
    {
        gimball.position = Vector3.zero;
        gimball.rotation = Quaternion.identity;
        gimball.localScale = Vector3.one;
    }

    // Update is called once per frame
    void Update()
    {
        if (objective != null)
        {
            RotateObject();
        }
    }
    private void RotateObject()
    {
        Vector3 currentMousePosition = Input.mousePosition;
        Vector3 mouseDelta = currentMousePosition - lastMousePosition; // Diferencia de movimiento del mouse
        lastMousePosition = currentMousePosition; // Actualiza la última posición del mouse

        float rotationAmount = mouseDelta.x * rotationSpeed; // Calcula la cantidad de rotación basada en el movimiento del mouse

        if (isOn2D)
        {
            // En 2D, rota alrededor del eje Z
            if(canRotateInZ)
                gimball.Rotate(0, 0, -rotationAmount); // El eje Z es para rotación en 2D
        }
        else
        {
            
            if(canRotateInY)// En 2.5D, rota alrededor del eje Y
                gimball.Rotate(0, -rotationAmount, 0); // El eje Y es para rotación en 3D
        }
    }

}
