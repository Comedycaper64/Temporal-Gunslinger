using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConquestAbility : RewindableAction
{
    private PlayerConquestAbility playerConquestAbility;
    private BulletPossessor playerBulletPossessor;
    private BulletPossessTarget previousTarget;
    private BulletPossessTarget daggerTarget;

    public static void ConquestAbilityUsed(
        GameObject dagger,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        BulletPossessor playerBulletPossessor,
        BulletPossessTarget previousTarget,
        PlayerConquestAbility playerConquestAbility
    )
    {
        new ConquestAbility(
            dagger,
            spawnPosition,
            spawnRotation,
            playerBulletPossessor,
            previousTarget,
            playerConquestAbility
        );
    }

    public ConquestAbility(
        GameObject dagger,
        Vector3 spawnPosition,
        Quaternion spawnRotation,
        BulletPossessor bulletPossessor,
        BulletPossessTarget previousTarget,
        PlayerConquestAbility playerConquestAbility
    )
    {
        this.previousTarget = previousTarget;
        playerBulletPossessor = bulletPossessor;
        this.playerConquestAbility = playerConquestAbility;

        dagger.transform.position = spawnPosition;
        dagger.transform.rotation = spawnRotation;

        daggerTarget = dagger.GetComponent<BulletPossessTarget>();

        BulletStateMachine daggerStateMachine = dagger.GetComponent<BulletStateMachine>();
        daggerStateMachine.SwitchToActive();

        Execute();
    }

    public override void Undo()
    {
        if (playerBulletPossessor.CheckPossessedBullet(daggerTarget))
        {
            playerBulletPossessor.UndoPossess(previousTarget);
        }

        playerConquestAbility.RefreshAbility();
    }
}
