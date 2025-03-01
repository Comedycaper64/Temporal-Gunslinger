using System;
using UnityEngine;

public class DeathScythe : MonoBehaviour, ISetAttacker
{
    [SerializeField]
    private WeakPoint scytheWeakPoint;

    [SerializeField]
    private Renderer scytheRenderer;
    private Material scytheMaterial;

    [SerializeField]
    private MeleeWeapon scytheMeleeWeapon;

    public event EventHandler<bool> OnAttackToggled;
    public event EventHandler<float> OnTimeOffset;

    private void Awake()
    {
        scytheMaterial = scytheRenderer.material;
        ToggleScythe(false);
    }

    public void ToggleScythe(bool toggle)
    {
        scytheWeakPoint.gameObject.SetActive(toggle);

        scytheMeleeWeapon.gameObject.SetActive(toggle);

        ToggleAttack(toggle);

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

    public void ToggleAttack(bool toggle)
    {
        OnAttackToggled?.Invoke(this, toggle);
    }

    public void SetTimeOffset(float offset)
    {
        OnTimeOffset?.Invoke(this, offset);
    }
}
