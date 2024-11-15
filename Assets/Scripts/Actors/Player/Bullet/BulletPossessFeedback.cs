using System;
using UnityEngine;
using UnityEngine.VFX;

public class BulletPossessFeedback : MonoBehaviour
{
    private VisualEffect possessEffect;

    private void Awake()
    {
        possessEffect = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        BulletPossessor.OnNewBulletPossessed += PlayEffect;
    }

    private void OnDisable()
    {
        BulletPossessor.OnNewBulletPossessed -= PlayEffect;
    }

    private void PlayEffect(object sender, BulletPossessTarget newTarget)
    {
        Vector3 movementDirection = (transform.position - newTarget.transform.position).normalized;

        possessEffect.SetVector3("WhooshDirection", movementDirection);
        possessEffect.Play();
    }
}
