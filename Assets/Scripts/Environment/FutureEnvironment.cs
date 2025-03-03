using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

using Random = UnityEngine.Random;

public class FutureEnvironment : RewindableMovement
{
    private bool reveal = false;

    //private bool unreveal = false;

    private int lastParticleSpawned = 0;
    private float particleSpawnTime = 1f;
    private float particleTimer = 0f;
    private float tweenTimer = 0f;
    private float revealRadius = 2.5f;
    private float timeToReveal = 0.05f;
    private Vector3 currentRevealPos = Vector3.zero;

    //private float timeToUnreveal = 0.025f;

    [SerializeField]
    private AnimationCurve revealCurve;

    [SerializeField]
    private List<VFXPlayback> futureVolumeVFX = new List<VFXPlayback>();

    private void Update()
    {
        particleTimer += GetSpeed() * Time.deltaTime;

        if (particleTimer > particleSpawnTime)
        {
            particleTimer = 0f;
            int index = lastParticleSpawned;
            while (index == lastParticleSpawned)
            {
                index = Random.Range(0, futureVolumeVFX.Count);
            }

            futureVolumeVFX[index].PlayEffect();
            lastParticleSpawned = index;
        }

        if (reveal)
        {
            tweenTimer += GetSpeed() * Time.deltaTime;

            //float lerp = Mathf.Lerp(0f, timeToReveal, tweenTimer);
            float lerp = Mathf.InverseLerp(0f, timeToReveal, tweenTimer);

            float curveValue = revealCurve.Evaluate(lerp);

            Shader.SetGlobalFloat("_RevealMaskRadius", curveValue * revealRadius);
            // float lerp = MMTween.Tween(
            //     tweenTimer,
            //     0f,
            //     timeToReveal,
            //     0f,
            //     revealRadius,
            //     MMTween.MMTweenCurve.EaseOutExponential
            // );

            // Shader.SetGlobalFloat("_RevealMaskRadius", lerp);

            // tweenTimer += GetSpeed() * Time.deltaTime;

            // if (tweenTimer >= timeToReveal)
            // {
            //     reveal = false;
            //     unreveal = true;
            //     tweenTimer = 0f;
            // }
        }
        // else if (unreveal)
        // {
        //     float lerp = MMTween.Tween(
        //         tweenTimer,
        //         0f,
        //         timeToUnreveal,
        //         revealRadius,
        //         0f,
        //         MMTween.MMTweenCurve.EaseInExponential
        //     );

        //     Shader.SetGlobalFloat("_RevealMaskRadius", lerp);

        //     tweenTimer += GetSpeed() * Time.deltaTime;

        //     if (tweenTimer >= timeToReveal)
        //     {
        //         unreveal = false;
        //         tweenTimer = 0f;
        //     }
        // }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        ImpactEffectFuture.OnHitEnvironment += SetNewRevealPosition;
        ToggleMovement(true);

        Shader.SetGlobalFloat("_RevealMaskRadius", 0f);
        Shader.SetGlobalVector("_RevealMaskPosition", Vector3.zero);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ImpactEffectFuture.OnHitEnvironment -= SetNewRevealPosition;
    }

    public void UndoReveal(float prevReveal, Vector3 revealPos)
    {
        if (prevReveal == -1)
        {
            reveal = false;
            currentRevealPos = Vector3.zero;
            Shader.SetGlobalFloat("_RevealMaskRadius", 0f);
            Shader.SetGlobalVector("_RevealMaskPosition", Vector3.zero);
            return;
        }

        tweenTimer = prevReveal;
        currentRevealPos = revealPos;
        Shader.SetGlobalVector("_RevealMaskPosition", currentRevealPos);
    }

    private void SetNewRevealPosition(object sender, Vector3 revealPos)
    {
        if (reveal == false)
        {
            tweenTimer = -1f;
        }

        reveal = true;
        //unreveal = false;

        FutureReveal.NewReveal(this, tweenTimer, currentRevealPos);

        tweenTimer = 0f;
        currentRevealPos = revealPos;

        Shader.SetGlobalFloat("_RevealMaskRadius", 0f);
        Shader.SetGlobalVector("_RevealMaskPosition", currentRevealPos);
    }
}
