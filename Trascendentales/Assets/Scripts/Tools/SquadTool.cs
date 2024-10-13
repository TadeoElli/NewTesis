using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SquadTool : Tools
{
    private Rigidbody objectiveRB;
    private Transform objectiveTr;
    [SerializeField] private Transform gimball, oldParent;
    [SerializeField] private IRotable rotable;
    [SerializeField] private IRotableClamp clamp;
    [SerializeField] private GameObject gimballGizmo2D, gimballGizmoPerspective;
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
        rotable = component;
        mouseState.SetLeftclickPress();
        rotable.SetGimballRef(gimball);
        objectiveTr = interactable.GetComponent<Transform>();
        playerController.OnPerspectiveSwitch += DropInteractable;
        playerController.OnLeftClickDrop += DropInteractable;
        playerController.OnToolSwitchCheck += DropInteractable;
        base.Interact(interactable, isPerspective2D); // Llama a la lógica común de interactuar
        canRotateInY = rotable.CanRotateInY();
        canRotateInZ = rotable.CanRotateInZ();
        Renderer objRenderer = objective.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            // Dimensiones reales del objeto en cada eje
            Vector3 objectSize = objRenderer.bounds.size;

            // Obtener el mayor valor entre X, Y, Z para mantener el gizmo como un círculo en cada eje
            float largestDimension = Mathf.Max(objectSize.x, objectSize.y, objectSize.z);

            if (isOn2D)
            {
                gimballGizmo2D.SetActive(true);
                gimballGizmo2D.transform.localScale = new Vector3(largestDimension / 3, largestDimension / 3, largestDimension / 3);
            }
            else
            {
                gimballGizmoPerspective.SetActive(true);
                gimballGizmoPerspective.transform.localScale = new Vector3(largestDimension / 3, largestDimension / 3, largestDimension / 3);
            }
        }
        objectiveRB = objective.GetComponent<Rigidbody>();
        objectiveRB.isKinematic = true;
        gimball.position = objectiveTr.position;
        if(objective.TryGetComponent<IRotableClamp>(out IRotableClamp component1))
        {
            gimball.rotation = objectiveTr.rotation;
            clamp = component1;
        }
        oldParent = objectiveTr.parent;
        objectiveTr.SetParent(gimball);
        lastMousePosition = Input.mousePosition;  // Guarda la posición actual del mouse al iniciar la interacción
    }
    public override void DropInteractable()
    {
        if(objective == null) return;
        objectiveRB.isKinematic = false;
        objectiveRB = null;
        objectiveTr.SetParent(oldParent);
        oldParent = null;
        objectiveTr = null;
        ResetGimball();
        mouseState.DropLeftClick();
        base.DropInteractable();
        rotable.SetGimballRef(null);
        playerController.OnPerspectiveSwitch -= DropInteractable;
        playerController.OnLeftClickDrop -= DropInteractable;
        playerController.OnToolSwitchCheck -= DropInteractable;
    }

    private void ResetGimball()
    {
        gimball.position = Vector3.zero;
        gimball.rotation = Quaternion.identity;
        gimball.localScale = Vector3.one;
        gimballGizmo2D.SetActive(false);   
        gimballGizmoPerspective.SetActive(false);
        gimballGizmo2D.transform.localScale = Vector3.one;
        gimballGizmoPerspective.transform.localScale = Vector3.one;
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
            if (canRotateInZ)
            {
                if (clamp != null && clamp.IsClamped())
                {
                    float newZRotation = gimball.localEulerAngles.z - rotationAmount; // Obtener la rotación actual en Z
                    newZRotation = ClampRotationAngle(newZRotation, clamp.GetMinRotationZ(), clamp.GetMaxRotationZ());
                    gimball.localEulerAngles = new Vector3(gimball.localEulerAngles.x, gimball.localEulerAngles.y, newZRotation);
                }
                else
                {
                    gimball.Rotate(0, 0, -rotationAmount); // El eje Z es para rotación en 2D
                }
            }
        }
        else
        {
            // En 2.5D, rota alrededor del eje Y
            if (canRotateInY)
            {
                if (clamp != null && clamp.IsClamped())
                {
                    float newYRotation = gimball.localEulerAngles.y - rotationAmount; // Obtener la rotación actual en Y
                    newYRotation = ClampRotationAngle(newYRotation, clamp.GetMinRotationY(), clamp.GetMaxRotationY());
                    gimball.localEulerAngles = new Vector3(gimball.localEulerAngles.x, newYRotation, gimball.localEulerAngles.z);
                }
                else
                {
                    gimball.Rotate(0, -rotationAmount, 0); // El eje Y es para rotación en 3D
                }
            }
        }
    }
    // Función auxiliar para clamping de ángulos de rotación entre un rango
    private float ClampRotationAngle(float angle, float min, float max)
    {
        if (angle > 180f) angle -= 360f; // Convertir el ángulo a un rango [-180, 180]
        return Mathf.Clamp(angle, min, max);
    }

}
