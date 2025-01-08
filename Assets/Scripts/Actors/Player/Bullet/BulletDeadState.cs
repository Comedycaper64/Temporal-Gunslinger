using UnityEngine;

public class BulletDeadState : State
{
    public static int bulletNumber;
    private Bullet bullet;
    private BulletStateMachine bulletStateMachine;
    private DissolveController dissolveController;

    public BulletDeadState(BulletStateMachine stateMachine)
        : base(stateMachine)
    {
        bullet = stateMachine.GetComponent<Bullet>();
        bulletStateMachine = stateMachine;
        if (!stateMachine.spawnedBullet && stateMachine.bCountAsAvailableBullet)
        {
            bulletNumber++;
        }

        dissolveController = stateMachine.GetDissolveController();
    }

    public override void Enter()
    {
        if (bulletStateMachine.bCountAsAvailableBullet)
        {
            bulletNumber--;
        }

        bullet.SetIsDead(true, bulletNumber > 0);

        if (dissolveController)
        {
            dissolveController.StartDissolve();
            //Debug.Log("Dissolve Start");
        }

        //stateMachine.ToggleInactive(true);
        if (bulletNumber <= 0)
        {
            GameManager.Instance.LevelLost();
        }
    }

    public override void Exit()
    {
        if (bulletStateMachine.bCountAsAvailableBullet)
        {
            bulletNumber++;
        }
        bullet.SetIsDead(false, false);

        if (dissolveController)
        {
            dissolveController.StopDissolve();
        }

        //stateMachine.ToggleInactive(false);
        //Debug.Log("Current bullet number: " + bulletNumber);
        if (bulletNumber <= 1)
        {
            //Debug.Log("Should be playin again");
            GameManager.Instance.UndoLevelLost();
        }
    }

    public override void Tick(float deltaTime) { }
}
