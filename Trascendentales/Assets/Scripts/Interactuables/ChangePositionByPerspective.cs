using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChangePositionByPerspective : MonoBehaviour
{
    float oldPositionZ;
    [SerializeField] CameraController cam;
    private ParentConstraint parentConstraint;

    private void OnEnable()
    {
        cam.OnCameraSwitch += ChangePerspective;
    }
    public void SetNewPosition()
    {
        if(TryGetComponent<ParentConstraint>(out ParentConstraint pConstraint))
        {
            pConstraint.constraintActive = false;
            pConstraint.locked = false;
        }
        oldPositionZ = transform.position.z;
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
        transform.position = new Vector3(transform.position.x,transform.position.y, oldPositionZ);
        if(TryGetComponent<ParentConstraint>(out ParentConstraint constraint))
        {
            constraint.constraintActive = true;
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
