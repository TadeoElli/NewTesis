using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawObjectTool : Tools
{

    [SerializeField] private GameObject cubePrefab, spherePrefab, rectanglePrefab; // Prefab del cubo a spawnear
    private GameObject selectedPrefab, spawnedObject;

    private bool isDrawing = false;


    public override void Awake()
    {
        base.Awake();
    }


    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        base.Interact(objective, isPerspective2D);
        mouseState.SetRightclickPress();
        playerController.OnRightClickDrop += DropInteractable; // Al soltar el clic derecho, limpiamos la interacción
        playerController.OnPerspectiveSwitch += ResetDrawing;
        playerController.OnPerspectiveSwitch += DropInteractable;
        playerController.OnToolSwitchCheck += ResetDrawing;
        playerController.OnToolSwitchCheck += DropInteractable;
        isDrawing = true;
    }
    private void SelectShape()
    {
        // Determinar la forma que se debe spawnear según las teclas de modificación
        if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
        {
            selectedPrefab = cubePrefab;
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            selectedPrefab = spherePrefab;
        }
        else if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        {
            selectedPrefab = rectanglePrefab;
        }
        else
        {
            // Si no hay teclas de modificación, no spawneamos nada
            selectedPrefab = null;
            return;
        }
    }
    private void ResetDrawing()
    {
        isDrawing = false;
    }

    void Update()
    {
        SelectShape();
        // Mientras se mantiene presionado, sigue ajustando el gizmo
        //UpdateDrawing();
    }

    public override void DropInteractable()
    {
        base.DropInteractable();
        if(isDrawing)
            FinishDrawing();
        mouseState.DropRightClick();
        playerController.OnRightClickDrop -= DropInteractable; // Al soltar el clic derecho, limpiamos la interacción
        playerController.OnPerspectiveSwitch -= ResetDrawing;
        playerController.OnPerspectiveSwitch -= DropInteractable;
        playerController.OnToolSwitchCheck -= ResetDrawing;
        playerController.OnToolSwitchCheck -= DropInteractable;
    }
    private void FinishDrawing()
    {
        isDrawing = false;
        // Obtener la posición en el mundo donde se va a spawnear el objeto
        Vector3 spawnPosition = GetMouseWorldPosition();
        // Si hay un objeto seleccionado, spawnearlo
        if (selectedPrefab != null)
        {
            if(spawnedObject != null)
                Destroy(spawnedObject);
            spawnedObject = Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
        }
    }
    private Vector3 GetMouseWorldPosition()
    {
        // Obtener la posición del mouse en el mundo según la perspectiva actual
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (isOn2D)
        {
            // Si estamos en 2D, utilizamos el plano X-Y
            Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0F));
            newPosition.z = playerController.transform.position.z;
            return newPosition;
        }
        else
        {
            // En 2.5D, hacemos un raycast para encontrar la posición en el mundo 3D
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, interactableLayer))
            {
                return hit.point; // Devuelve el punto donde el raycast intersectó un objeto
            }
            else
            {
                Vector3 newPosition = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0F));
                newPosition.z = playerController.transform.position.z;
                return newPosition;
            }
        }
    }

}
