using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    private float dissolveRate = 0.02f;

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
        StartCoroutine(Dissolve());
    }

    private IEnumerator Dissolve()
    {
        float counter = 0;

        while (counter < 1)
        {
            counter += dissolveRate;
            foreach (Material material in materials)
            {
                material.SetFloat("_Dissolve_Amount", counter);
            }
            yield return new WaitForSeconds(0.025f);
        }
    }
}
