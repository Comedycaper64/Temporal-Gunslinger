public class EnemyFamineStateMachine : EnemyMeleeStateMachine
{
    //private Transform enemyStartPosition;

    // [SerializeField]
    // private List<GameObject> bodyColliders = new List<GameObject>();

    private void Start()
    {
        SwitchState(stateDictionary[StateEnum.inactive]);
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.active, new EnemyFamineActiveState(this));
        stateDictionary.Add(StateEnum.dead, new EnemyDeadState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        // foreach (GameObject collider in bodyColliders)
        // {
        //     collider.SetActive(!toggle);
        // }
    }

    // public void ResetPosition()
    // {
    //     transform.position = enemyStartPosition.position;
    //     //SetRunAnimationExitTime(0f);
    // }

    // public bool HasStartPosition()
    // {
    //     return enemyStartPosition;
    // }

    // public void SetStartPosition(Transform newStart)
    // {
    //     enemyStartPosition = newStart;
    // }
}
