using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuskBodyHitSFX : MonoBehaviour
{
    [SerializeField]
    private List<StrongPoint> bodyColliders = new List<StrongPoint>();

    [SerializeField]
    private AudioClip hitSFX;

    private void OnEnable()
    {
        foreach (StrongPoint collider in bodyColliders)
        {
            collider.OnHit += PlayHitSFX;
        }
    }

    private void OnDisable()
    {
        foreach (StrongPoint collider in bodyColliders)
        {
            collider.OnHit -= PlayHitSFX;
        }
    }

    private void PlayHitSFX(object sender, EventArgs e)
    {
        AudioManager.PlaySFX(hitSFX, 0.5f, 1, transform.position);
    }
}
