using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableAnimator : RewindableMovement
{
    [SerializeField]
    private Animator animator;

    private void Start()
    {
        ToggleMovement(true);
    }

    private void Update()
    {
        animator.SetFloat("animSpeed", GetSpeed());
        //Debug.Log("Speed: " + GetSpeed());
    }
}
