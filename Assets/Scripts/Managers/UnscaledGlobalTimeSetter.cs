using UnityEngine;

[ExecuteAlways]
public class UnscaledGlobalTimeSetter : MonoBehaviour
{
    void Update()
    {
        Shader.SetGlobalFloat("_UnscaledTime", Time.unscaledTime);
    }
}
