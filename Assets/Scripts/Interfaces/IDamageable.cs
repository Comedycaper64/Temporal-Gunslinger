using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void ProjectileHit(out float velocityConservation, out bool bIsPassable);
}
