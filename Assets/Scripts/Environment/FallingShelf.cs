using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingShelf : MonoBehaviour, IReactable
{
    //private bool bFalling = false;
    private int RestingHash;
    private int FallingHash;

    private Animator shelfAnimator;

    [SerializeField]
    private WeakPoint shelfSupport;

    [SerializeField]
    private GameObject[] shelfDamagers;

    private void Awake()
    {
        shelfSupport.OnHit += StartFalling;
        shelfAnimator = GetComponent<Animator>();
        RestingHash = Animator.StringToHash("Falling Shelf Rest");
        FallingHash = Animator.StringToHash("Falling Shelf Fall");
    }

    private void OnDisable()
    {
        shelfSupport.OnHit -= StartFalling;
    }

    private void StartFalling(object sender, EventArgs e)
    {
        GameObject weakPoint = (sender as WeakPoint).gameObject;
        DestroyWeakPoint.WeakPointDestroyed(this, weakPoint);

        //Start falling
        shelfAnimator.CrossFade(FallingHash, 0f, 0);
        foreach (GameObject damager in shelfDamagers)
        {
            damager.SetActive(true);
        }
    }

    public void UndoReaction()
    {
        //stop falling
        shelfAnimator.CrossFade(RestingHash, 0f, 0);
        foreach (GameObject damager in shelfDamagers)
        {
            damager.SetActive(false);
        }
    }
}
