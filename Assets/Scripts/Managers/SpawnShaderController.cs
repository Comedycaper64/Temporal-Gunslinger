using MoreMountains.Tools;
using UnityEngine;

public class SpawnShaderController : MonoBehaviour
{
    private bool bOnOpeningChange = false;
    private float maskOpening = 0.5f;
    private float reaperOpening = 0.95f;
    private float targetOpening = 0f;
    private float startOpening = 0f;
    private float openingSpeed = 0.75f;
    private float tweenTimer = 0f;

    [SerializeField]
    private float openingOverride = -1f;

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
        if (openingOverride >= 0f)
        {
            spawnShader.SetFloat("_Size", openingOverride);
            return;
        }

        if (bOnOpeningChange)
        {
            float newSize = MMTween.Tween(
                tweenTimer,
                0f,
                1f,
                startOpening,
                targetOpening,
                MMTween.MMTweenCurve.EaseInCubic
            );
            spawnShader.SetFloat("_Size", newSize);

            tweenTimer += openingSpeed * Time.unscaledDeltaTime;

            if (tweenTimer > 1f)
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
        tweenTimer = 0f;
    }

    public void ReaperOpen()
    {
        startOpening = targetOpening;
        targetOpening = reaperOpening;
        bOnOpeningChange = true;
        tweenTimer = 0f;
    }

    public void CloseOpening()
    {
        startOpening = targetOpening;
        targetOpening = 0f;
        bOnOpeningChange = true;
        tweenTimer = 0f;
    }
}
