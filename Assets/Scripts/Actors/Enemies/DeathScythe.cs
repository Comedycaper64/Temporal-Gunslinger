using UnityEngine;

public class DeathScythe : MonoBehaviour
{
    [SerializeField]
    private WeakPoint scytheWeakPoint;

    [SerializeField]
    private Renderer scytheRenderer;
    private Material scytheMaterial;

    [SerializeField]
    private MeleeWeapon scytheMeleeWeapon;

    private void Awake()
    {
        scytheMaterial = scytheRenderer.material;
        ToggleScythe(false);
    }

    public void ToggleScythe(bool toggle)
    {
        scytheWeakPoint.gameObject.SetActive(toggle);

        scytheMeleeWeapon.gameObject.SetActive(toggle);

        if (toggle)
        {
            scytheMaterial.SetFloat("_Artifact_Glow", 1f);
        }
        else
        {
            scytheMaterial.SetFloat("_Artifact_Glow", 0f);
        }
    }

    public WeakPoint GetScytheWeakPoint()
    {
        return scytheWeakPoint;
    }
}
