using Cinemachine;
using UnityEngine;

public class BulletCameraController : MonoBehaviour
{
    [SerializeField]
    private CinemachineFreeLook bulletCamera;

    // public Quaternion GetCameraRotation()
    // {
    //     return Quaternion.LookRotation(Camera.main.transform.forward);
    // }

    public void SetCameraAxisValues(Vector2 axisValues)
    {
        bulletCamera.m_XAxis.Value = axisValues.x;
        bulletCamera.m_YAxis.Value = axisValues.y;
    }

    public Vector2 GetCameraAxisValues()
    {
        return new Vector2(bulletCamera.m_XAxis.Value, bulletCamera.m_YAxis.Value);
    }

    public void ToggleCamera(bool toggle)
    {
        bulletCamera.gameObject.SetActive(toggle);
    }
}
