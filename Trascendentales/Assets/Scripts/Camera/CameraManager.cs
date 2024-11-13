using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    InputManager inputManager;

    [SerializeField] private Transform targetTransform;
    [SerializeField] private CinemachineVirtualCamera virtualCamera2DFront, virtualCamera2DBack;
    [SerializeField] private CinemachineVirtualCamera virtualCamera3DFront, virtualCamera3DBack;
    private CinemachineVirtualCamera currentVirtualCamera;
    [SerializeField] private float transitionDuration = 1f; // Duración del slerp
    public bool is2D = false;
    private Camera cam;
    private bool hasToCheck = false;
    private Coroutine currentTransition;

    public bool isFrontView = true;
    // Valores de offset para el eje Z
    [SerializeField] private float frontViewOffsetZ = -14f;
    [SerializeField] private float backViewOffsetZ = 14f;
    public event Action OnCameraSwitch;

    // Lista de objetos que pueden verse afectados por el cambio de perspectiva
    private List<IObjectAffectableByPerspective> affectableObjects = new List<IObjectAffectableByPerspective>();

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        inputManager.OnPerspectiveSwitch += TogglePerspective;
        inputManager.OnChangeCameraAngle += ChangeCameraAngle;
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        cam = GetComponent<Camera>();
        // Asegúrate de que solo una cámara esté activa al inicio
        ResetVirtualCameras();
    }
    private void FixedUpdate()
    {
        currentVirtualCamera.transform.position = new Vector3(targetTransform.transform.position.x, targetTransform.transform.position.y, -14f);
    }
    private void ResetVirtualCameras()
    {
        virtualCamera2DFront.gameObject.SetActive(false);
        virtualCamera2DBack.gameObject.SetActive(false);
        virtualCamera3DBack.gameObject.SetActive(false);
        virtualCamera3DFront.gameObject.SetActive(false);
        if (is2D)
        {
            if(isFrontView)
            {
                virtualCamera2DFront.gameObject.SetActive(true);
                currentVirtualCamera = virtualCamera2DFront;
            }
            else
            {
                virtualCamera2DBack.gameObject.SetActive(true);
                currentVirtualCamera = virtualCamera2DBack;
            }
        }
        else
        {
            if (isFrontView)
            {
                virtualCamera3DFront.gameObject.SetActive(true);
                currentVirtualCamera = virtualCamera3DFront;
            }
            else
            {
                virtualCamera3DBack.gameObject.SetActive(true);
                currentVirtualCamera = virtualCamera3DBack;
            }
        }
    }

    private void ChangeCameraAngle()
    {
        // Puedes agregar lógica aquí si deseas cambiar el ángulo de la cámara, pero 
        // si solo estás cambiando entre 2D y 3D, podrías dejar esto vacío o simplificarlo
        isFrontView = !isFrontView;
        ResetVirtualCameras();
        NotifyObjectsOfPerspectiveChange();
    }

    private void TogglePerspective()
    {
        if (currentTransition != null)
        {
            StopCoroutine(currentTransition);
        }

        is2D = !is2D;
        hasToCheck = true;
        ResetVirtualCameras();
        OnCameraSwitch?.Invoke();

        // Iniciar la transición suave
        currentTransition = StartCoroutine(TransitionCamera());
        StartCoroutine(CheckForCameraChange());
    }
    private IEnumerator CheckForCameraChange()
    {
        while (hasToCheck)
        {
            // Revisa la condición para el cambio de perspectiva
            if ((is2D && cam.orthographic) || (!is2D && !cam.orthographic))
            {
                NotifyObjectsOfPerspectiveChange();
                hasToCheck = false;  // Termina la verificación
            }
            else
            {
                yield return new WaitForFixedUpdate(); // Espera al siguiente frame fijo
            }
        }
    }
    private IEnumerator TransitionCamera()
    {
        // Obtener las posiciones y rotaciones actuales de las cámaras
        Transform startTransform = currentVirtualCamera.transform;
        Transform endTransform = is2D ? (isFrontView ? virtualCamera2DFront.transform : virtualCamera2DBack.transform ):(isFrontView ? virtualCamera3DFront.transform : virtualCamera3DBack.transform);

        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            t = Mathf.SmoothStep(0f, 1f, t); // Usar SmoothStep para una transición suave

            // Interpolación esférica para la rotación
            transform.position = Vector3.Lerp(startTransform.position, endTransform.position, t);
            transform.rotation = Quaternion.Slerp(startTransform.rotation, endTransform.rotation, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Asegurarse de que la cámara termine exactamente en la posición y rotación finales
        transform.position = endTransform.position;
        transform.rotation = endTransform.rotation;
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
