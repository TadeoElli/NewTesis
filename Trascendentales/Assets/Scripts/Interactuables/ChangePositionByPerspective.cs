using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChangePositionByPerspective : MonoBehaviour, IObjectAffectableByPerspective
{
    private float originalPositionZ;
    private bool isOriginalPositionSaved = false;
    private CameraManager cam;

    [SerializeField] private bool isInFront;  // Si el objeto puede ser visto desde la vista frontal
    [SerializeField] private bool isInBack;   // Si el objeto puede ser visto desde la vista trasera


    private void Awake()
    {
        cam = FindObjectOfType<CameraManager>();
    }
    private void OnEnable()
    {
        // Registra el objeto en el CameraManager
        cam.RegisterObject(this);
        SaveOriginalPositionZ();
    }

    private void OnDisable()
    {
        // Desregistra el objeto en el CameraManager
        cam.UnregisterObject(this);
    }
    private void SaveOriginalPositionZ()
    {
        if (!isOriginalPositionSaved)
        {
            originalPositionZ = transform.position.z; // Guarda la posición Z original la primera vez
            isOriginalPositionSaved = true;
        }
    }
    public void OnPerspectiveChanged(bool is2D, bool isFrontView)
    {
        // Verifica si el objeto debe ser visible o no y ajusta la posición en Z
        if (is2D)
        {
            if ((isFrontView && isInFront) || (!isFrontView && isInBack) || (isInFront && isInBack))
            {
                SetNewPosition();
            }
            else
            {
                ReturnToOriginalPosition();
            }
        }
        else
        {
            // Restaura la posición original si no está en 2D
            ReturnToOriginalPosition();
        }
    }
    private void SetNewPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void ReturnToOriginalPosition()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, originalPositionZ);
    }

}

public interface IObjectAffectableByPerspective
{
    void OnPerspectiveChanged(bool is2D, bool isFrontView);
}