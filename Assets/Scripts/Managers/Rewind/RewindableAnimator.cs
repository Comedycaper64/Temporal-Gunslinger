using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableAnimator : RewindableMovement
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float animationSpeedModifier;

    private void Start()
    {
        ToggleMovement(true);
    }

    private void Update()
    {
        float animSpeed = Mathf.Clamp(GetSpeed() * animationSpeedModifier, -1f, 1f);
        animator.SetFloat("animSpeed", animSpeed);
        //Debug.Log("Speed: " + GetSpeed());
    }
}
