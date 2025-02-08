using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Tools;
using UnityEngine;

using Random = UnityEngine.Random;

public class FutureEnvironment : RewindableMovement
{
    private bool reveal = false;
    private bool unreveal = false;

    private int lastParticleSpawned = 0;
    private float particleSpawnTime = 1f;
    private float particleTimer = 0f;
    private float tweenTimer = 0f;
    private float revealRadius = 2.5f;
    private float timeToReveal = 0.025f;
    private float timeToUnreveal = 0.025f;

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
            float lerp = MMTween.Tween(
                tweenTimer,
                0f,
                timeToReveal,
                0f,
                revealRadius,
                MMTween.MMTweenCurve.EaseOutExponential
            );

            Shader.SetGlobalFloat("_RevealMaskRadius", lerp);

            tweenTimer += GetSpeed() * Time.deltaTime;

            if (tweenTimer >= timeToReveal)
            {
                reveal = false;
                unreveal = true;
                tweenTimer = 0f;
            }
        }
        else if (unreveal)
        {
            float lerp = MMTween.Tween(
                tweenTimer,
                0f,
                timeToUnreveal,
                revealRadius,
                0f,
                MMTween.MMTweenCurve.EaseInExponential
            );

            Shader.SetGlobalFloat("_RevealMaskRadius", lerp);

            tweenTimer += GetSpeed() * Time.deltaTime;

            if (tweenTimer >= timeToReveal)
            {
                unreveal = false;
                tweenTimer = 0f;
            }
        }
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

    private void SetNewRevealPosition(object sender, Vector3 revealPos)
    {
        reveal = true;
        unreveal = false;
        tweenTimer = 0f;

        Shader.SetGlobalFloat("_RevealMaskRadius", 0f);
        Shader.SetGlobalVector("_RevealMaskPosition", revealPos);
    }
}
