using UnityEngine;

public class TimescaleSetter : RewindableMovement
{
    [SerializeField]
    private Renderer shaderRenderer;

    private Material shader;

    private void Awake()
    {
        if (!shaderRenderer)
        {
            shaderRenderer = GetComponent<Renderer>();
        }

        shader = shaderRenderer.material;
        ToggleMovement(true);
    }

    private void Update()
    {
        shader.SetFloat("_Timescale", GetSpeed());
    }
}
