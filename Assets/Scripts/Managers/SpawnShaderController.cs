using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnShaderController : MonoBehaviour
{
    private bool bOnOpeningChange = false;
    private float maskOpening = 0.25f;
    private float reaperOpening = 1f;
    private float targetOpening = 0f;
    private float startOpening;
    private float openingSpeed = 0.5f;

    [SerializeField]
    private MeshRenderer spawnMesh;
    private Material spawnShader;

    private void Awake()
    {
        spawnShader = spawnMesh.material;
        spawnShader.SetFloat("_Size", 0f);
    }

    private void Update()
    {
        if (bOnOpeningChange)
        {
            float currentSize = spawnShader.GetFloat("_Size");
            float lerpRatio =
                1
                - (
                    Mathf.Abs(currentSize - targetOpening) / Mathf.Abs(startOpening - targetOpening)
                );
            //Debug.Log("A: " + (bulletCamera.m_Lens.FieldOfView - targetFOV));
            //Debug.Log("B: " + (nonTargetFOV - targetFOV));
            //Debug.Log("Ratio: " + lerpRatio);
            float newSize = Mathf.Lerp(
                startOpening,
                targetOpening,
                lerpRatio + (openingSpeed * Time.unscaledDeltaTime)
            );
            spawnShader.SetFloat("_Size", newSize);

            if (Mathf.Abs(targetOpening - newSize) < 0.1f)
            {
                bOnOpeningChange = false;
            }
        }
    }

    public void MaskOpen()
    {
        startOpening = targetOpening;
        targetOpening = maskOpening;
        bOnOpeningChange = true;
    }

    public void ReaperOpen()
    {
        startOpening = targetOpening;
        targetOpening = reaperOpening;
        bOnOpeningChange = true;
    }

    public void CloseOpening()
    {
        startOpening = targetOpening;
        targetOpening = 0f;
        bOnOpeningChange = true;
    }
}
