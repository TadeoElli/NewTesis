using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawObjectTool : Tools
{

    [SerializeField] private GameObject cubePrefab, spherePrefab, rectanglePrefab; // Prefab del cubo a spawnear
    [SerializeField] private GameObject cubeFeedback, sphereFeedback, rectangleFeedback; // Prefab del cubo a spawnear
    private GameObject selectedPrefab,selectedFeedback, spawnedObject;

    private bool isDrawing = false;
    // Variables para el temporizador
    [SerializeField] private float cooldown = 2f; // Tiempo de espera en segundos
    private bool canInteract = true;

    public override void Awake()
    {
        base.Awake();
        //DeactivateAllFeedback(); // Desactivamos todos los feedbacks al inicio
    }
    private void Start()
    {
        SelectCube();
    }


    public override void Interact(GameObject objective, bool isPerspective2D)
    {
        if (!canInteract) return; // Si el temporizador no ha terminado, no se puede interactuar

        base.Interact(objective, isPerspective2D);
        mouseState.SetRightclickPress();
        inputManager.OnRightClickDrop += DropInteractable; // Al soltar el clic derecho, limpiamos la interacción
        inputManager.OnPerspectiveSwitch += ResetDrawing;
        inputManager.OnPerspectiveSwitch += DropInteractable;
        inputManager.OnToolSwitchCheck += ResetDrawing;
        inputManager.OnToolSwitchCheck += DropInteractable;
        isDrawing = true;
        StartCoroutine(CooldownTimer()); // Iniciar el temporizador

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
        MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.BrushBox);
        //ResetDrawing();
    }
    public void SelectSphere()
    {
        selectedPrefab = spherePrefab;
        MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.BrushSphere);
        //ResetDrawing();
    }
    public void SelectPlatform()
    {
        selectedPrefab = rectanglePrefab;
        MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.BrushRectangle);
        //ResetDrawing();
    }
    private void ResetDrawing()
    {
        isDrawing = false;
        DeactivateAllFeedback();
    }

    void FixedUpdate()
    {
        if (MouseState.Instance.CurrentToolActive() != ToolTypes.Brush)
        {
            DeactivateAllFeedback();
            return;
        }
        if (isDrawing)
        {
            SelectShape();

            if (selectedFeedback != null)
            {
                UpdateFeedbackPosition();
            }
        }
    }

    public override void DropInteractable()
    {
        //base.DropInteractable();
        if (isDrawing)
        {
            // Si el clic derecho se suelta antes de alcanzar el umbral, spawneamos el objeto seleccionado
            if (selectedPrefab != null)
            {
                FinishDrawing();
            }
        }
        ResetDrawing();
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
    private IEnumerator CooldownTimer()
    {
        canInteract = false;
        yield return new WaitForSeconds(cooldown);
        canInteract = true;
    }
    public override void SetCurrentAlternativeTool()
    {
        if(selectedPrefab == cubePrefab)
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.BrushBox);
        else if(selectedPrefab == spherePrefab)
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.BrushSphere);
        else if(selectedPrefab == rectanglePrefab)
            MouseState.Instance.SetCurrentAlternativeToolType(AlternativeToolTypes.BrushRectangle);
    }

}
