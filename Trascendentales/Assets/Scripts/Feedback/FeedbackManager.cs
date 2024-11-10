using Unity.VisualScripting;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance { get; private set; }
    private LineRenderer lineRenderer;
    private Vector3 initialMousePosition; // Posición inicial del mouse
    private bool isDrawingLineFromMouse = false; // Flag para saber si estamos dibujando la línea desde el mouse
    private GameObject parentObject; // Primer objeto para conectar
    private GameObject childObject;  // Segundo objeto para conectar
    private bool isConnectingObjects = false; // Flag para saber si estamos conectando objetos


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // Asegurarnos de que la línea esté desactivada al inicio
    }
    private void Update()
    {
        // Actualiza la línea si estamos dibujando desde el mouse
        if (isDrawingLineFromMouse)
        {
            UpdateMouseLine();
        }

        // Actualiza la línea si estamos conectando dos objetos
        if (isConnectingObjects && parentObject != null && childObject != null)
        {
            UpdateObjectConnectionLine();
        }
    }

    // ==================== Líneas desde el mouse ====================

    // Activar LineRenderer entre el clic inicial y el mouse
    public void StartMouseLine(GameObject gameObject)
    {
        DeactivateLineRenderer();
        initialMousePosition = GetMouseWorldPosition();
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, initialMousePosition);
        lineRenderer.enabled = true;
        isDrawingLineFromMouse = true;
    }

    // Desactivar LineRenderer del mouse
    public void StopMouseLine()
    {
        isDrawingLineFromMouse = false;
        lineRenderer.enabled = false;
    }
    // Actualiza la posición de la línea mientras el mouse se mueve
    private void UpdateMouseLine()
    {
        Vector3 currentMousePosition = GetMouseWorldPosition();
        lineRenderer.SetPosition(1, currentMousePosition); // Actualiza el segundo punto de la línea con la posición actual del mouse
    }

    // Aplicar feedback según el tipo de interacción
    public void ApplyFeedback(Renderer rend, Color feedbackColor)
    {
        if (rend != null && rend.materials.Length > 1) // Asegurarse de que hay más de un material
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block, 1); // Obtiene el MaterialPropertyBlock para el segundo material (índice 1)

            block.SetColor("_LineColor", feedbackColor); // Cambia el color del segundo material
            rend.SetPropertyBlock(block, 1);         // Aplica los cambios al segundo material
        }
    }

    // Limpiar el feedback (quitar material)
    public void ClearFeedback(Renderer rend)
    {
        if (rend != null && rend.materials.Length > 1) // Asegurarse de que hay más de un material
        {
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            rend.GetPropertyBlock(block, 1); // Obtiene el MaterialPropertyBlock para el segundo material (índice 1)

            block.SetColor("_LineColor", new Color(0, 0, 0, 0)); // Color transparente para el segundo material
            rend.SetPropertyBlock(block, 1);                 // Aplica los cambios al segundo material
        }
    }

    // ==================== Líneas entre dos objetos ====================

    // Activar LineRenderer entre dos GameObjects
    public void ActivateLineRenderer(GameObject parent, GameObject child)
    {
        DeactivateLineRenderer();
        parentObject = parent;
        childObject = child;
        isConnectingObjects = true;
        lineRenderer.SetPosition(0, parent.transform.position);
        lineRenderer.SetPosition(1, child.transform.position);
        lineRenderer.enabled = true;
    }

    // Desactivar LineRenderer entre objetos
    public void DeactivateLineRenderer()
    {
        isConnectingObjects = false;
        lineRenderer.enabled = false;
        parentObject = null;
        childObject = null;
    }
    // Actualiza la posición de la línea entre los dos objetos
    private void UpdateObjectConnectionLine()
    {
        if (parentObject != null && childObject != null)
        {
            lineRenderer.SetPosition(0, parentObject.transform.position); // Actualiza la posición del primer objeto
            lineRenderer.SetPosition(1, childObject.transform.position);  // Actualiza la posición del segundo objeto
        }
    }
    // ==================== Funciones auxiliares ====================

    // Obtener la posición del mouse en el mundo
    private Vector3 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = 10f; // Distancia desde la cámara al punto en el mundo
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
