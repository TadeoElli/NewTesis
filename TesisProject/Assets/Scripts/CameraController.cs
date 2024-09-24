using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerController playerController;
    private bool is2D = false;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    [SerializeField] private float orthographicSize = 5f; // Tamaño de la cámara ortográfica en modo 2D
    [SerializeField]private Vector3 camera2DPosition = new Vector3(0, 10, -10); // Posición de la cámara en modo 2D
    [SerializeField]private Quaternion camera2DRotation = Quaternion.Euler(90, 0, 0); // Cámara mirando hacia abajo en modo 2D

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerController.OnPerspectiveSwitch += TogglePerspective;
    }
    private void Start()
    {
        // Guardar la posición y rotación inicial de la cámara para el modo 2.5D
        initialPosition = mainCamera.transform.position;
        initialRotation = mainCamera.transform.rotation;
    }


    private void TogglePerspective()
    {
        if (is2D)
        {
            // Volver a 2.5D
            mainCamera.orthographic = false;
            mainCamera.transform.position = initialPosition;
            mainCamera.transform.rotation = initialRotation;
        }
        else
        {
            // Cambiar a 2D
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = orthographicSize;
            mainCamera.transform.position = camera2DPosition;
            mainCamera.transform.rotation = camera2DRotation;
        }

        is2D = !is2D;
    }
}
