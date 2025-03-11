using System.Collections.Generic;
using UnityEngine;

public class DeathDissolveController : DissolveController
{
    [SerializeField]
    private float dissolveOverride;
    private MaterialPropertyBlock propBlock;

    protected override void Start()
    {
        base.Start();
        meshMaterials = GetMeshMaterials();
    }

    public override void StartDissolve(float targetDissolve = 1)
    {
        dissolveInitialState = dissolveValue;
        DeathDissolve.Dissolved(this, dissolveInitialState, 0f, 0f, true);
        base.StartDissolve(targetDissolve);
    }

    public override void StopDissolve()
    {
        DeathDissolve.Dissolved(this, dissolveInitialState, dissolveTarget, counter, false);
        ToggleMovement(false);
        counter = 0;
    }

    protected override void Update()
    {
        if (dissolveOverride >= 0f)
        {
            foreach (Material material in meshMaterials)
            {
                material.SetFloat("_Dissolve_Amount", dissolveOverride);
            }
        }
        else
        {
            base.Update();
        }
    }

    protected override void Dissolve()
    {
        counter += dissolveRate * GetSpeed() * Time.deltaTime;
        float newCounter = Mathf.Clamp(counter, 0f, 1f);

        // dissolveValue = Mathf.LerpUnclamped(dissolveInitialState, dissolveTarget, counter);
        // float dissolve = Mathf.Lerp(dissolveInitialState, dissolveTarget, newCounter);
        dissolveValue = Mathf.Lerp(dissolveInitialState, dissolveTarget, newCounter);

        // Debug.Log("Counter: " + counter);
        //Debug.Log("Dissolve Value: " + dissolveValue);
        // Debug.Log("Dissolve: " + dissolve);

        foreach (Material material in meshMaterials)
        {
            material.SetFloat("_Dissolve_Amount", dissolveValue);
        }

        // foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        // {
        //     skinnedMeshRenderer.GetPropertyBlock(propBlock);
        //     propBlock.SetFloat("_Dissolve_Amount", dissolveValue);
        //     skinnedMeshRenderer.SetPropertyBlock(propBlock);
        // }

        // foreach (MeshRenderer meshRenderer in meshRenderers)
        // {
        //     meshRenderer.GetPropertyBlock(propBlock);
        //     propBlock.SetFloat("_Dissolve_Amount", dissolveValue);
        //     meshRenderer.SetPropertyBlock(propBlock);
        // }

        if (newCounter >= 1f)
        {
            StopDissolve();
        }
    }

    // protected override Material[] GetMeshMaterials()
    // {
    //     Material[] materials = base.GetMeshMaterials();
    //     List<MaterialPropertyBlock> blocks = new List<MaterialPropertyBlock>();

    //     foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
    //     {
    //         MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
    //         skinnedMeshRenderer.GetPropertyBlock(propBlock);
    //         blocks.Add(propBlock);
    //     }

    //     foreach (MeshRenderer meshRenderer in meshRenderers)
    //     {
    //         MaterialPropertyBlock propBlock = new MaterialPropertyBlock();
    //         meshRenderer.GetPropertyBlock(propBlock);
    //         blocks.Add(propBlock);
    //     }

    //     materialPropertyBlocks = blocks.ToArray();

    //     return materials;
    // }

    protected override void GameManager_OnGameStateChange(object sender, StateEnum e) { }

    public void UndoDissolve(float dissolveState)
    {
        ToggleMovement(false);
        counter = 0;
        SetDissolve(dissolveState);
    }

    public void UndoDissolveEnd(float initialState, float target, float dissolveCounter)
    {
        dissolveInitialState = initialState;
        base.StartDissolve(target);
        counter = dissolveCounter;
    }
}
