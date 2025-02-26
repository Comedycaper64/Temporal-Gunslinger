using System.Collections.Generic;
using UnityEngine;

public class DissolveController : RewindableMovement
{
    protected float dissolveRate = 100f;
    protected float counter = 0;
    protected float dissolveValue = 0f;
    protected float dissolveInitialState = 0f;
    protected float dissolveTarget = 1f;

    // [SerializeField]
    // private float dissolveTargetOverride = -1;

    [SerializeField]
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    [SerializeField]
    private MeshRenderer[] meshRenderers;

    [SerializeField]
    private GameObject[] manualDisables;

    protected Material[] meshMaterials;

    [SerializeField]
    private AudioClip dissolveSFX;

    [SerializeField]
    private VFXPlayback dissolveVFX;

    private void Start()
    {
        // foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        // {
        //     materials.Add(skinnedMeshRenderer.material);
        // }
        // foreach (MeshRenderer meshRenderer in meshRenderers)
        // {
        //     materials.Add(meshRenderer.material);
        // }

        // if (dissolveTargetOverride > 0f)
        // {
        //     dissolveTarget = dissolveTargetOverride;
        // }

        GameManager.OnGameStateChange += GameManager_OnGameStateChange;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        GameManager.OnGameStateChange -= GameManager_OnGameStateChange;
    }

    private void GameManager_OnGameStateChange(object sender, StateEnum e)
    {
        if (e == StateEnum.idle)
        {
            foreach (Material material in GetMeshMaterials())
            {
                material.SetFloat("_Dissolve_Amount", 0f);
            }
        }
    }

    private void Update()
    {
        if (IsActive())
        {
            Dissolve();
        }
    }

    public virtual void StartDissolve(float targetDissolve = 1f)
    {
        ToggleMovement(true);
        counter = 0;
        dissolveTarget = targetDissolve;

        meshMaterials = GetMeshMaterials();

        foreach (GameObject gameObject in manualDisables)
        {
            gameObject.SetActive(false);
        }

        if (dissolveSFX)
        {
            AudioManager.PlaySFX(dissolveSFX, 0.5f, 0, transform.position);
        }

        if (dissolveVFX)
        {
            dissolveVFX.PlayEffect();
        }
    }

    public virtual void StopDissolve()
    {
        ToggleMovement(false);
        counter = 0;

        foreach (Material material in GetMeshMaterials())
        {
            material.SetFloat("_Dissolve_Amount", counter);
        }

        foreach (GameObject gameObject in manualDisables)
        {
            gameObject.SetActive(true);
        }
    }

    public void SetDissolve(float targetDissolve)
    {
        meshMaterials = GetMeshMaterials();
        dissolveValue = targetDissolve;

        foreach (Material material in meshMaterials)
        {
            material.SetFloat("_Dissolve_Amount", targetDissolve);
        }
    }

    protected Material[] GetMeshMaterials()
    {
        List<Material> materials = new List<Material>();

        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            materials.Add(skinnedMeshRenderer.material);
        }

        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            materials.Add(meshRenderer.material);
        }

        return materials.ToArray();
    }

    protected virtual void Dissolve()
    {
        counter += dissolveRate * GetSpeed() * Time.deltaTime;
        float newCounter = Mathf.Clamp(counter, 0f, dissolveTarget);

        foreach (Material material in meshMaterials)
        {
            //Debug.Log("Material: " + material.name + " dissolve at " + newCounter);
            material.SetFloat("_Dissolve_Amount", newCounter);
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
