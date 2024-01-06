using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongPoint : MonoBehaviour, IDamageable
{
    public void ProjectileHit(Bullet projectile)
    {
        //Currently not good, breaks bullet rewind
        projectile.BulletImpact();
    }
}
