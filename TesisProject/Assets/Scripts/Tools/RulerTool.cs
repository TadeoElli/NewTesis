using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RulerTool : Tools
{
    private Rigidbody objectiveRB;
    private Transform objectiveTr;
    [SerializeField] private Transform gimball;
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
        playerController.OnToolDesinteract += DropInteractable;
        playerController.OnPerspectiveSwitch += DropInteractable;
        minScale = component.GetMinScale();
        maxScale = component.GetMaxScale();
        base.Interact(interactable, isPerspective2D); // Llama a la lógica común de interactuar
        //Desactivo la colision
        objectiveRB = objective.GetComponent<Rigidbody>();
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
        objectiveTr.SetParent(gimball);
        gimball.localScale = objectiveTr.localScale;
        gimball.localRotation = objectiveTr.localRotation;
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
        playerController.OnToolDesinteract -= DropInteractable;
        playerController.OnPerspectiveSwitch -= DropInteractable;
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
        // Lógica específica de la regla: Escalar el objeto
        if (objective == null)
        return;
        Vector3 currentMousePosition = Input.mousePosition;
        float scaleFactor = (currentMousePosition.x - initialMousePosition.x) * mouseSensitiviy;

        if (isOn2D)
        {
            Vector3 newScale = initialScale + new Vector3(scaleFactor, scaleFactor, 0f);
            newScale = new Vector3(Mathf.Clamp(newScale.x, minScale.x, maxScale.x), Mathf.Clamp(newScale.y, minScale.y, maxScale.y), Mathf.Clamp(newScale.z, minScale.z, maxScale.z));
            gimball.localScale = newScale;
        }
        else
        {
            Vector3 newScale = initialScale + new Vector3(scaleFactor, scaleFactor, scaleFactor);
            newScale = new Vector3(Mathf.Clamp(newScale.x, minScale.x, maxScale.x), Mathf.Clamp(newScale.y, minScale.y, maxScale.y), Mathf.Clamp(newScale.z, minScale.z, maxScale.z));
            gimball.localScale = newScale;
        }
        
    }
    
}
