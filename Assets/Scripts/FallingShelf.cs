using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingShelf : MonoBehaviour
{
    //private bool bFalling = false;
    private int RestingHash;
    private int FallingHash;

    private Animator shelfAnimator;

    [SerializeField]
    private WeakPoint shelfSupport;

    [SerializeField]
    private GameObject shelfDamager;

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
        shelfDamager.SetActive(true);
    }

    public void UndoFall()
    {
        //stop falling
        shelfAnimator.CrossFade(RestingHash, 0f, 0);
        shelfDamager.SetActive(false);
    }
}
