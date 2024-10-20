using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    [SerializeField] private Camera mainCamera;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private List<Transform> camera3Dpositions;
    [SerializeField] private List<Transform> camera2Dpositions;
    private bool is2D = false;
    [SerializeField] private float orthographicSize = 7f; // Tamaño de la cámara ortográfica en modo 2D
    public event Action<bool> OnCameraSwitch;
    private int index = 0;

    private void Awake()
    {
        
        playerController = FindObjectOfType<PlayerController>();
        playerController.OnPerspectiveSwitch += TogglePerspective;
        playerController.OnChangeCameraToLeft += RotateToLeft;
        playerController.OnChangeCameraToRight += RotateToRight;
    }
    private void Start()
    {
        mainCamera.transform.position = camera3Dpositions[0].position;
        mainCamera.transform.rotation = camera3Dpositions[0].rotation;
    }
    private void RotateToLeft()
    {
        index++;
        if (index > 3)
            index = 0;
    }
    private void RotateToRight()
    {
        index--;
        if (index < 0)
            index = 3;
    }
    private void TogglePerspective()
    {
        if (is2D)
        {
            // Volver a 2.5D
            mainCamera.orthographic = false;
        }
        else
        {
            // Cambiar a 2D
            mainCamera.orthographic = true;
            mainCamera.orthographicSize = orthographicSize;
        }
        is2D = !is2D;
        OnCameraSwitch?.Invoke(is2D);
    }
    private void Update()
    {
        if (is2D)
        {
            transform.position = camera2Dpositions[index].position;
            transform.rotation = camera2Dpositions[index].rotation;
        }
        else
        {
            transform.position = camera3Dpositions[index].position;
            transform.rotation = camera3Dpositions[index].rotation;
        }
    }
}