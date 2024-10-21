using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField]private Transform targetTransform;
    [SerializeField]private Camera mainCamera;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    [SerializeField] private List<Transform> camera_3D_positions;
    [SerializeField] private List<Transform> camera_2D_positions;
    [SerializeField] private Transform cameraPivot;
    private bool is2D = false;
    [SerializeField] private float orthographicSize = 7f; // Tamaño de la cámara ortográfica en modo 2D
    private int index = 0;
    public bool isFrontView = true;

    public float cameraFollowSpeed = 0.2f;

    // Lista de objetos que pueden verse afectados por el cambio de perspectiva
    private List<IObjectAffectableByPerspective> affectableObjects = new List<IObjectAffectableByPerspective>();

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        inputManager.OnPerspectiveSwitch += TogglePerspective;
        inputManager.OnChangeCameraAngle += ChangeCameraAngle;
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cameraPivot.transform.position = camera_3D_positions[index].transform.position;
        cameraPivot.transform.rotation = camera_3D_positions[index].transform.rotation;

    }

    public void FollowTarget()
    {
        if (is2D)
        {
            cameraPivot.position = camera_2D_positions[index].position;
            cameraPivot.rotation = camera_2D_positions[index].rotation;
        }
        else
        {
            cameraPivot.position = camera_3D_positions[index].position;
            cameraPivot.rotation = camera_3D_positions[index].rotation;
        }
        Vector3 targetPosition = Vector3.SmoothDamp
            (transform.position, targetTransform.position, ref cameraFollowVelocity, cameraFollowSpeed);

        transform.position = targetPosition;
        
    }
    private void ChangeCameraAngle()
    {
        index++;
        if(index > 1)
            index = 0;
        isFrontView = !isFrontView;
        // Notifica a los objetos sobre el cambio de ángulo de cámara
        NotifyObjectsOfPerspectiveChange();
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
        NotifyObjectsOfPerspectiveChange();
    }
    public void RegisterObject(IObjectAffectableByPerspective obj)
    {
        if (!affectableObjects.Contains(obj))
        {
            affectableObjects.Add(obj);
        }
    }

    public void UnregisterObject(IObjectAffectableByPerspective obj)
    {
        if (affectableObjects.Contains(obj))
        {
            affectableObjects.Remove(obj);
        }
    }

    private void NotifyObjectsOfPerspectiveChange()
    {
        foreach (var obj in affectableObjects)
        {
            obj.OnPerspectiveChanged(is2D, isFrontView);
        }
    }
}
