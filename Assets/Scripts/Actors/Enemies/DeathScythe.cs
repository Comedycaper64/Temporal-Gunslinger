using System;
using UnityEngine;

public class DeathScythe : MonoBehaviour, ISetAttacker
{
    [SerializeField]
    private WeakPoint[] scytheWeakPoints;

    [SerializeField]
    private Renderer scytheRenderer;
    private Material scytheMaterial;

    [SerializeField]
    private MeleeWeapon scytheMeleeWeapon;

    public event Action OnHit;
    public event EventHandler<bool> OnAttackToggled;
    public event EventHandler<float> OnTimeOffset;

    private void Awake()
    {
        scytheMaterial = scytheRenderer.material;
        ToggleScythe(false);
    }

    private void OnDisable()
    {
        foreach (WeakPoint weakPoint in scytheWeakPoints)
        {
            weakPoint.OnHit -= ScytheHit;
        }
    }

    public void ToggleScythe(bool toggle)
    {
        foreach (WeakPoint weakPoint in scytheWeakPoints)
        {
            weakPoint.gameObject.SetActive(toggle);

            if (toggle)
            {
                weakPoint.OnHit += ScytheHit;
            }
            else
            {
                weakPoint.OnHit -= ScytheHit;
            }
        }

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

    private void ScytheHit(object sender, EventArgs e)
    {
        OnHit?.Invoke();
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
