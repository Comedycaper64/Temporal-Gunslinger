using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class DialogueCameraDirector : MonoBehaviour
{
    private CameraMode currentCameraMode = CameraMode.none;

    private Dictionary<CameraMode, CinemachineVirtualCamera> modeToCamera =
        new Dictionary<CameraMode, CinemachineVirtualCamera>();

    [SerializeField]
    private CinemachineVirtualCamera fullBodyCamera;

    [SerializeField]
    private CinemachineVirtualCamera faceZoomCamera;

    [SerializeField]
    private CinemachineVirtualCamera wideAngleCamera;

    private void Awake()
    {
        modeToCamera.Add(CameraMode.fullbody, fullBodyCamera);
        modeToCamera.Add(CameraMode.faceZoom, faceZoomCamera);
        modeToCamera.Add(CameraMode.wideAngle, wideAngleCamera);

        TurnOffAllCameras();
    }

    public void ChangeCameraMode(CameraMode cameraMode, Transform actorTransform)
    {
        if (cameraMode == currentCameraMode)
        {
            return;
        }
        TurnOffAllCameras();

        if (cameraMode == CameraMode.none)
        {
            return;
        }
        CinemachineVirtualCamera activeCamera = modeToCamera[cameraMode];
        activeCamera.enabled = true;
        activeCamera.Follow = actorTransform;
        activeCamera.LookAt = actorTransform;
        currentCameraMode = cameraMode;
    }

    private void TurnOffAllCameras()
    {
        fullBodyCamera.enabled = false;
        faceZoomCamera.enabled = false;
        wideAngleCamera.enabled = false;
    }

    private void ClearCurrentCamera()
    {
        currentCameraMode = CameraMode.none;
    }

    public void EndOfDialogueCleanup()
    {
        ChangeCameraMode(CameraMode.none, transform);
        ClearCurrentCamera();
    }
}
