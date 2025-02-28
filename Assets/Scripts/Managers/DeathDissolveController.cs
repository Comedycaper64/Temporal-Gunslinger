using UnityEngine;

public class DeathDissolveController : DissolveController
{
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

        if (newCounter >= 1f)
        {
            StopDissolve();
        }
    }

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
