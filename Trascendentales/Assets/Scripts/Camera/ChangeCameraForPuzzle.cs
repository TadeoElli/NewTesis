using UnityEngine;
using UnityEngine.Events;

public class ChangeCameraForPuzzle : MonoBehaviour
{
    [SerializeField] FollowPlayerCamera cameraLookUp, camera3DF, camera3DB, camera2DF, camera2DB;
    [SerializeField] private Transform newCameraLook, newCamera3DFPos, newCamera3DBPos, newCamera2DFPos, newCamera2DBPos;

    public void SetNewCameraPosition()
    {
        cameraLookUp.SetNewTransform(newCameraLook);
        camera3DF.SetNewTransform(newCamera3DFPos);
        camera3DB.SetNewTransform(newCamera3DBPos);
        camera2DF.SetNewTransform(newCamera2DFPos);
        camera2DB.SetNewTransform(newCamera2DBPos);
    }
    public void FollowPlayer()
    {
        camera3DF.FollowPlayer();
        camera3DB.FollowPlayer();
        camera2DF.FollowPlayer();
        camera2DB.FollowPlayer();
        cameraLookUp.FollowPlayer();
    }

}
