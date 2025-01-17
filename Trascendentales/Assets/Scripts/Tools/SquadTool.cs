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
    private Vector3 initialMousePosition;                  // Última posición del mouse para calcular el movimiento
    private bool canRotateInY, canRotateInZ;
    [SerializeField] private float rotationSteps, rotationThreshold;
    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject interactable, bool isPerspective2D)
    {
        if(!interactable.TryGetComponent<IRotable>(out IRotable component))
            return;
        rotable = component;
        if(!rotable.CanRotate())
            return;
        mouseState.SetLeftclickPress();
        rotable.SetGimballRef(gimball);
        objectiveTr = interactable.GetComponent<Transform>();
        inputManager.OnPerspectiveSwitch += DropInteractable;
        inputManager.OnLeftClickDrop += DropInteractable;
        inputManager.OnToolSwitchCheck += DropInteractable;
        rotable.SetIsRotating(true);
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
        if(!rotable.NeedToBeKinematic())
            objectiveRB.isKinematic = true;
        gimball.position = objectiveTr.position;
        if(objective.TryGetComponent<IRotableClamp>(out IRotableClamp component1))
        {
            gimball.rotation = objectiveTr.rotation;
            clamp = component1;
        }
        oldParent = objectiveTr.parent;
        objectiveTr.SetParent(gimball);
        initialMousePosition = Input.mousePosition;  // Guarda la posición actual del mouse al iniciar la interacción
    }
    public override void DropInteractable()
    {
        if(objective == null) return;
        if(!rotable.NeedToBeKinematic())
            objectiveRB.isKinematic = false;
        objectiveRB = null;
        rotable.SetIsRotating(false);
        objectiveTr.SetParent(oldParent);
        oldParent = null;
        objectiveTr = null;
        clamp = null;
        ResetGimball();
        mouseState.DropLeftClick();
        base.DropInteractable();
        rotable.SetGimballRef(null);
        inputManager.OnPerspectiveSwitch -= DropInteractable;
        inputManager.OnLeftClickDrop -= DropInteractable;
        inputManager.OnToolSwitchCheck -= DropInteractable;
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
    void FixedUpdate()
    {
        if (objective != null)
        {
            if (!rotable.CanRotate())
            {
                DropInteractable();
                return;
            }
            RotateObject();
        }
    }
    private void RotateObject()
    {
        Vector3 currentMousePosition = Input.mousePosition; // Current mouse position

        // Calculate the distance moved by the mouse from the initial position
        float distanceMoved = (currentMousePosition - initialMousePosition).magnitude;

        // Check if the distance moved exceeds the threshold
        if (distanceMoved > rotationThreshold)
        {
            // Determine how many steps to rotate based on distance moved
            int stepsToRotate = Mathf.FloorToInt(distanceMoved / rotationThreshold); // Calculate steps based on threshold

            float rotationAmount = stepsToRotate * rotationSteps; // Rotate by 15 degrees per step
            rotationAmount = currentMousePosition.x > initialMousePosition.x ? rotationAmount : rotationAmount * -1;
            if (isOn2D)
            {
                if (!cameraManager.isFrontView)
                    rotationAmount *= -1;

                if (canRotateInZ)
                {
                    if (clamp != null && clamp.IsClamped())
                    {
                        float newZRotation = gimball.localEulerAngles.z - rotationAmount; // Get current Z rotation
                        newZRotation = ClampRotationAngle(newZRotation, clamp.GetMinRotationZ(), clamp.GetMaxRotationZ());
                        gimball.localEulerAngles = new Vector3(gimball.localEulerAngles.x, gimball.localEulerAngles.y, newZRotation);
                    }
                    else
                    {
                        gimball.rotation = Quaternion.Euler(0, 0, -rotationAmount); // Rotate around Z axis for 2D
                    }
                }
            }
            else
            {
                // In 2.5D, rotate around Y axis
                if (canRotateInY)
                {
                    if (clamp != null && clamp.IsClamped())
                    {
                        float newYRotation = ClampRotationAngle(-rotationAmount, clamp.GetMinRotationY(), clamp.GetMaxRotationY()); // Get current Y rotation
                        gimball.rotation = Quaternion.Euler(0, -newYRotation, 0); // Rotate around Y axis for 3D
                    }
                    else
                    {
                        gimball.rotation = Quaternion.Euler(0, -rotationAmount, 0); // Rotate around Y axis for 3D
                    }
                }
            }
        }
    }
    // Función auxiliar para clamping de ángulos de rotación entre un rango
    private float ClampRotationAngle(float angle, float min, float max)
    {
        //if (angle > 180f) angle -= 360f; // Convertir el ángulo a un rango [-180, 180]
        return Mathf.Clamp(angle, min, max);
    }

}
