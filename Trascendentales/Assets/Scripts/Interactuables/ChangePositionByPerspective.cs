using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChangePositionByPerspective : MonoBehaviour
{
    float oldPositionZ;
    [SerializeField] CameraManager cam;
    private ParentConstraint parentConstraint;
    private bool isOriginalPositionSaved = false;

    private void Awake()
    {
        cam = FindObjectOfType<CameraManager>();
    }
    private void OnEnable()
    {
        cam.OnCameraSwitch += ChangePerspective;
    }
    private void SaveOriginalPositionZ()
    {
        if (!isOriginalPositionSaved)
        {
            oldPositionZ = transform.position.z; // Guarda la posición Z original la primera vez
            isOriginalPositionSaved = true;
        }
    }
    public void SetNewPosition()
    {
        SaveOriginalPositionZ(); // Solo guarda la posición original si no lo ha hecho ya
        if (TryGetComponent<ParentConstraint>(out ParentConstraint pConstraint))
        {
            pConstraint.constraintActive = false;
            pConstraint.locked = false;
        }
        transform.position = new Vector3(transform.position.x,transform.position.y,0f);
        if (pConstraint != null)
        {
            Vector3 positionOffset = transform.InverseTransformPoint(pConstraint.GetSource(0).sourceTransform.position);
            Vector3 rotationOffset = new Vector3(90,0,-90);
            pConstraint.SetTranslationOffset(0, positionOffset);
            pConstraint.SetRotationOffset(0, rotationOffset);
            pConstraint.constraintActive = true;
            pConstraint.locked = true;
        }

    }
    public void ReturnToOldPosition()
    {
        // Restaura la posición Z original al cambiar a 2.5D
        transform.position = new Vector3(transform.position.x, transform.position.y, oldPositionZ);

        // Reactiva el constraint si existe
        if (parentConstraint != null)
        {
            parentConstraint.constraintActive = true;
        }
    }
    public void ChangePerspective(bool isOn2D)
    {
        if(isOn2D)
            SetNewPosition();
        else
            ReturnToOldPosition();
    }

    private void OnDisable()
    {
        cam.OnCameraSwitch -= ChangePerspective;
    }
}
