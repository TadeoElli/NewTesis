using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulerTool : Tools
{
    private Transform objectiveTr;
    private IEscalable scalable;
    [SerializeField] private Transform gimball, oldParent;
    [SerializeField] private GameObject gimballGizmo2D, gimballGizmoPerspective;
    private Vector3 initialScale;
    private Vector3 initialMousePosition;
    [Header("Parameters")]
    private Vector3 minScale; // Escala mínima permitida
    private Vector3 maxScale; // Escala máxima permitida
    [SerializeField] private float mouseSensitiviy = 0.01f;

    public override void Awake()
    {
        base.Awake();
    }

    public override void Interact(GameObject interactable, bool isPerspective2D)
    {
        if(!interactable.TryGetComponent<IEscalable>(out IEscalable component)) 
            return;
        if (!component.CanScale())
            return;
        scalable = component;
        mouseState.SetLeftclickPress();
        inputManager.OnLeftClickDrop += DropInteractable;
        inputManager.OnToolSwitchCheck += DropInteractable;
        inputManager.OnPerspectiveSwitch += DropInteractable;
        scalable.SetIsScaling(true);
        minScale = scalable.GetMinScale();
        maxScale = scalable.GetMaxScale();
        base.Interact(interactable, isPerspective2D); // Llama a la lógica común de interactuar
        //Desactivo la colision
        if(objective.TryGetComponent<Rigidbody>(out Rigidbody objectiveRB))
            objectiveRB.isKinematic = true;
        //Seteo el Gimball
        objectiveTr = objective.GetComponent<Transform>();
        SetGimball();
        //Reseteo el transform del objetivo
        objectiveTr.localScale = Vector3.one;
        objectiveTr.localRotation = Quaternion.identity;
        //Establezco las variables iniciales
        initialScale = gimball.localScale;
        initialMousePosition = Input.mousePosition;
    }
    private void SetGimball()
    {
        gimball.position = objectiveTr.position;
        oldParent = objectiveTr.parent;
        objectiveTr.SetParent(gimball);
        // Obtener el tamaño real del objeto usando su renderer
        Renderer objRenderer = objectiveTr.GetComponent<Renderer>();
        if (objRenderer != null)
        {
            Vector3 objectSize = objRenderer.bounds.size;
            float largestDimension = Mathf.Max(objectSize.x, objectSize.y, objectSize.z);

            // Ajustar la escala del gimball para que no dependa de la escala actual del objeto
            gimball.localScale = new Vector3(largestDimension, largestDimension, largestDimension);
        }

        gimball.localRotation = objectiveTr.localRotation;

        if (isOn2D)
        {
            gimballGizmo2D.SetActive(true);
            AdjustGizmoScale(gimballGizmo2D);
        }
        else
        {
            gimballGizmo2D.SetActive(true);
            gimballGizmoPerspective.SetActive(true);
            AdjustGizmoScale(gimballGizmo2D);
            AdjustGizmoScale(gimballGizmoPerspective);
        }
    }
    // Método para ajustar la escala del gizmo de acuerdo a la escala del gimball
    private void AdjustGizmoScale(GameObject gizmo)
    {
        // Ajustamos la escala del gizmo para que no se vea afectada por la escala del gimball
        Vector3 adjustedScale = objectiveTr.localScale; // La escala deseada (escala del objeto)
        gizmo.transform.localScale = new Vector3(
            adjustedScale.x / gimball.localScale.x,
            adjustedScale.y / gimball.localScale.y,
            adjustedScale.z / gimball.localScale.z
        );
        Vector3 scale = gizmo.transform.localScale;
        scale = scale / 5;
        scale = new Vector3(Mathf.Clamp(scale.x, 0.3f, 1), Mathf.Clamp(scale.y, 0.3f, 1), Mathf.Clamp(scale.z, 0.3f, 1));
        gizmo.transform.localScale = scale;
    }
    public override void DropInteractable()
    {
        if(objective == null) return;
        if(objective.TryGetComponent<Rigidbody>(out Rigidbody objectiveRB))
            objectiveRB.isKinematic = false;
        scalable.SetIsScaling(false);
        scalable = null;
        mouseState.DropLeftClick();
        objectiveTr.SetParent(oldParent);
        oldParent = null;
        objectiveTr = null;
        ResetGimball();
        base.DropInteractable();
        inputManager.OnLeftClickDrop -= DropInteractable;
        inputManager.OnToolSwitchCheck -= DropInteractable;
        inputManager.OnPerspectiveSwitch -= DropInteractable;
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
        // Lógica específica de la regla: Escalar el objeto
        if (objective == null)
            return;

        Vector3 currentMousePosition = Input.mousePosition;
        float scaleFactorX = (currentMousePosition.x - initialMousePosition.x) * mouseSensitiviy;
        float scaleFactorY = (currentMousePosition.y - initialMousePosition.y) * mouseSensitiviy;

        if (isOn2D)
        {
            Vector3 newScale = initialScale + new Vector3(scaleFactorX + scaleFactorY, scaleFactorX + scaleFactorY, 0f);
            newScale = new Vector3(Mathf.Clamp(newScale.x, minScale.x, maxScale.x), Mathf.Clamp(newScale.y, minScale.y, maxScale.y), Mathf.Clamp(newScale.z, minScale.z, maxScale.z));
            gimball.localScale = newScale;
        }
        else
        {
            Vector3 newScale = initialScale + new Vector3(scaleFactorX + scaleFactorY, scaleFactorX + scaleFactorY, scaleFactorX + scaleFactorY);
            newScale = new Vector3(Mathf.Clamp(newScale.x, minScale.x, maxScale.x), Mathf.Clamp(newScale.y, minScale.y, maxScale.y), Mathf.Clamp(newScale.z, minScale.z, maxScale.z));
            gimball.localScale = newScale;
        }
        
    }
    
}
