using UnityEngine;
using UnityEngine.UI;

public class UnscaledTimeSetterUI : MonoBehaviour
{
    [SerializeField]
    private Image imageRenderer;

    private Material shader;

    private void Awake()
    {
        shader = imageRenderer.material;
    }

    private void Update()
    {
        shader.SetFloat("_UnscaledTime", Time.unscaledTime);
    }
}
