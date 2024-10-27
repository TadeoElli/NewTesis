using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawObjectTool : Tools
{

    [SerializeField] private GameObject cubePrefab, spherePrefab, rectanglePrefab; // Prefab del cubo a spawnear
    [SerializeField] private GameObject cubeFeedback, sphereFeedback, rectangleFeedback; // Prefab del cubo a spawnear
    private GameObject selectedPrefab,selectedFeedback, spawnedObject;

    private bool isDrawing = false;
    private bool isShowing = false;
    private bool isShowingSelectionWheel = false;
    private float rightClickHoldTime = 0f;
    private float holdThreshold = 0.5f; // Tiempo requerido para mostrar la rueda de selección


    public override void Awake()
    {
        base.Awake();
        selectedPrefab = cubePrefab;
        DeactivateAllFeedback(); // Desactivamos todos los feedbacks al inicio
    }


    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        base.Interact(objective, isPerspective2D);
        mouseState.SetRightclickPress();
        inputManager.OnRightClickDrop += DropInteractable; // Al soltar el clic derecho, limpiamos la interacción
        inputManager.OnPerspectiveSwitch += ResetDrawing;
        inputManager.OnPerspectiveSwitch += DropInteractable;
        inputManager.OnToolSwitchCheck += ResetDrawing;
        inputManager.OnToolSwitchCheck += DropInteractable;
        isDrawing = true;
        rightClickHoldTime = 0f; // Reiniciar el temporizador al interactuar
    }
    private void SelectShape()
    {
        ActivateFeedback(GetFeedbackForPrefab(selectedPrefab));
    }

    private GameObject GetFeedbackForPrefab(GameObject prefab)
    {
        if (prefab == cubePrefab) return cubeFeedback;
        if (prefab == spherePrefab) return sphereFeedback;
        if (prefab == rectanglePrefab) return rectangleFeedback;
        return null;
    }
    public void SelectCube()
    {
        selectedPrefab = cubePrefab;
        inputManager.HideDrawObjectWheel();
        ResetDrawing();
    }
    public void SelectSphere()
    {
        selectedPrefab = spherePrefab;
        inputManager.HideDrawObjectWheel();
        ResetDrawing();
    }
    public void SelectPlatform()
    {
        selectedPrefab = rectanglePrefab;
        inputManager.HideDrawObjectWheel();
        ResetDrawing();
    }
    private void ResetDrawing()
    {
        isDrawing = false;
        isShowingSelectionWheel = false;
        rightClickHoldTime = 0f;
        DeactivateAllFeedback();
    }

    void Update()
    {
        if (MouseState.Instance.CurrentToolActive() != ToolTypes.Brush)
        {
            DeactivateAllFeedback();
            return;
        }
        if (isDrawing)
        {
            rightClickHoldTime += Time.deltaTime;

            // Muestra la rueda de selección si se mantiene presionado el clic derecho el tiempo suficiente
            if (rightClickHoldTime >= holdThreshold && !isShowingSelectionWheel)
            {
                inputManager.ShowDrawObjectWheel();
                isShowingSelectionWheel = true;
            }
            else
            {
                SelectShape();
            }

            if (selectedFeedback != null)
            {
                UpdateFeedbackPosition();
            }
        }
    }

    public override void DropInteractable()
    {
        base.DropInteractable();
        if (isDrawing)
        {
            // Si el clic derecho se suelta antes de alcanzar el umbral, spawneamos el objeto seleccionado
            if (rightClickHoldTime < holdThreshold && selectedPrefab != null)
            {
                FinishDrawing();
            }
            ResetDrawing();
            inputManager.HideDrawObjectWheel();
        }
        mouseState.DropRightClick();
        inputManager.OnRightClickDrop -= DropInteractable; // Al soltar el clic derecho, limpiamos la interacción
        inputManager.OnPerspectiveSwitch -= ResetDrawing;
        inputManager.OnPerspectiveSwitch -= DropInteractable;
        inputManager.OnToolSwitchCheck -= ResetDrawing;
        inputManager.OnToolSwitchCheck -= DropInteractable;

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
            newPosition.z = inputManager.transform.position.z;
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
                newPosition.z = inputManager.transform.position.z;
                return newPosition;
            }
        }
    }
    private void UpdateFeedbackPosition()
    {
        // Actualizar la posición del feedback activo
        if (selectedFeedback != null)
        {
            Vector3 mousePosition = GetMouseWorldPosition();
            selectedFeedback.transform.position = mousePosition;
            selectedFeedback.SetActive(true);
        }
    }

    private void ActivateFeedback(GameObject feedback)
    {
        // Desactivar el feedback anterior
        if (selectedFeedback != feedback)
        {
            DeactivateAllFeedback();
            selectedFeedback = feedback;
        }
        selectedFeedback.SetActive(true);
    }

    private void DeactivateAllFeedback()
    {
        // Desactivar todos los objetos de feedback
        cubeFeedback.SetActive(false);
        sphereFeedback.SetActive(false);
        rectangleFeedback.SetActive(false);
        selectedFeedback = null;
    }

}
