public class BulletActiveState : State
{
    Bullet bullet;

    public BulletActiveState(BulletStateMachine stateMachine)
        : base(stateMachine)
    {
        bullet = stateMachine.GetComponent<Bullet>();
    }

    public override void Enter()
    {
        bullet.ToggleBulletActive(true);
    }

    public override void Exit() { }

    public override void Tick(float deltaTime) { }
}
