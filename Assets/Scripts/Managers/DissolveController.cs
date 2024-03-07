using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : RewindableMovement
{
    private float dissolveRate = 1f;
    private float counter = 0;

    [SerializeField]
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    [SerializeField]
    private MeshRenderer[] meshRenderers;

    private List<Material> materials = new List<Material>();

    private void Start()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            materials.Add(skinnedMeshRenderer.material);
        }
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            materials.Add(meshRenderer.material);
        }
    }

    private void Update()
    {
        Dissolve();
    }

    public void StartDissolve()
    {
        ToggleMovement(true);
    }

    public void StopDissolve()
    {
        ToggleMovement(false);
    }

    private void Dissolve()
    {
        if (GetUnscaledSpeed() == 0f)
        {
            return;
        }

        float newCounter = counter + (dissolveRate * GetSpeed());
        counter = Mathf.Clamp01(newCounter);

        foreach (Material material in materials)
        {
            material.SetFloat("_Dissolve_Amount", counter);
        }
        //yield return new WaitForSeconds(0.025f);
    }

    // private IEnumerator UnDissolve()
    // {
    //     float counter = 1;

    //     while (counter > 0)
    //     {
    //         counter -= dissolveRate;
    //         foreach (Material material in materials)
    //         {
    //             material.SetFloat("_Dissolve_Amount", counter);
    //         }
    //         yield return new WaitForSeconds(0.025f);
    //     }
    // }
}
