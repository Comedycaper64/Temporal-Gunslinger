using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class DamageEnemy : RewindableAction
{
    private float halfDissolve = 0.25f;
    private ActorResilientHealth resilientHealth;
    private DissolveController dissolveController;

    public static void EnemyDamaged(
        ActorResilientHealth resilientHealth,
        DissolveController dissolveController
    )
    {
        new DamageEnemy(resilientHealth, dissolveController);
    }

    private DamageEnemy(ActorResilientHealth resilientHealth, DissolveController dissolveController)
    {
        this.resilientHealth = resilientHealth;
        this.dissolveController = dissolveController;

        dissolveController.StartDissolve(halfDissolve);

        Execute();
    }

    public override void Undo()
    {
        resilientHealth.UndoDamage();
        dissolveController.StopDissolve();
    }
}
