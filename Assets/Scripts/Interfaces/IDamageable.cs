using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ProjectileHit(MeleeWeapon weapon);
    void ProjectileHit(Bullet projectile);
}
