using Cinemachine;
using UnityEngine;

public class CinemachineExtensionLayerMask : CinemachineExtension
{
    [SerializeField]
    private LayerMask _layers;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime
    )
    {
        //Camera.main.cullingMask = _layers;
    }

    public override bool OnTransitionFromCamera(
        ICinemachineCamera fromCam,
        Vector3 worldUp,
        float deltaTime
    )
    {
        Camera.main.cullingMask = _layers;
        return base.OnTransitionFromCamera(fromCam, worldUp, deltaTime);
    }
}
