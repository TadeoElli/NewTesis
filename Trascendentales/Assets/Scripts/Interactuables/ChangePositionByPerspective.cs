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
        if(TryGetComponent<ParentConstraint>(out ParentConstraint constraint))
        {
            constraint.constraintActive = false;
        }
        oldPositionZ = transform.position.z;
        transform.position = new Vector3(transform.position.x,transform.position.y,0f);
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
