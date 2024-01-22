using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DialogueCameraDirector : MonoBehaviour
{
    private CameraMode currentCameraMode;

    [SerializeField]
    private CinemachineVirtualCamera virtualCamera;

    public void ChangeCameraMode(CameraMode cameraMode, Transform actorTransform)
    {
        if (cameraMode == currentCameraMode)
        {
            return;
        }
    }
}
