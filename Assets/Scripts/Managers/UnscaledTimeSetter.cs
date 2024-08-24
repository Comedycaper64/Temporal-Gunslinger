using UnityEngine;

public class UnscaledTimeSetter : MonoBehaviour
{
    [SerializeField]
    private Renderer shaderRenderer;

    private Material shader;

    private void Awake()
    {
        shader = shaderRenderer.material;
    }

    private void Update()
    {
        shader.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
