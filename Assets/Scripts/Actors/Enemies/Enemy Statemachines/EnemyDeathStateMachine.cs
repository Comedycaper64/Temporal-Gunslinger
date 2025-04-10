using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathStateMachine : StateMachine, IReactable
{
    public float etherealDissolveValue = 0.25f;
    private int flowIndex = 0;
    private int quillIndex = 0;
    private int deadZoneIndex = 0;
    private LockOnTarget revenantTarget;
    private List<State> stateFlow = new List<State>();
    private Stack<float> stateAnimationTimes = new Stack<float>();
    private Stack<float> stateDurationTimes = new Stack<float>();

    [SerializeField]
    private Transform restPosition;

    [SerializeField]
    private Transform scytheAPosition;

    [SerializeField]
    private Transform scytheBPosition;

    [SerializeField]
    private Transform[] quillPositions;

    [SerializeField]
    private DeathScythe deathScythe;

    [SerializeField]
    private FutureShield[] deathShields;

    [SerializeField]
    private BulletStateMachine[] deathQuills;

    [SerializeField]
    private FingerOfDeath deathSpell;

    [SerializeField]
    private DeathDeadzone[] deathDeadzones;

    [SerializeField]
    private AudioClip[] deathBossVoiceLines;

    [SerializeField]
    private string[] deathBossLineText;

    [SerializeField]
    private AudioSource deathSource;

    [SerializeField]
    private DeathHealth health;

    [SerializeField]
    private GameObject weakPoint;

    [SerializeField]
    private VFXPlayback deathVFX;

    [SerializeField]
    private RewindState rewindState;

    private void Start()
    {
        revenantTarget = GameManager.GetRevenant().GetComponent<LockOnTarget>();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SwitchState(new EnemyInactiveState(this));
    }

    public override void ToggleInactive(bool toggle)
    {
        deathVFX.StopEffect();

        if (toggle)
        {
            foreach (DeathDeadzone deadzone in deathDeadzones)
            {
                deadzone.ResetZone();
            }
        }
    }

    protected override void SetupDictionary()
    {
        stateDictionary.Add(StateEnum.inactive, new EnemyInactiveState(this));
        stateDictionary.Add(StateEnum.idle, new EnemyDeathIdleState(this));
        stateDictionary.Add(StateEnum.active, new EnemyDeathRestingState(this, true));
        stateDictionary.Add(StateEnum.dead, new EnemyBossDeadState(this));

        // stateFlow.Add(new EnemyDeathScytheAAltState(this));
        // stateFlow.Add(new EnemyDeathScytheBAltState(this));
        // stateFlow.Add(new EnemyDeathDeadzonesState(this));
        // stateFlow.Add(new EnemyDeathHeavyCastState(this));
        // stateFlow.Add(new EnemyDeathQuillFlurryState(this));

        stateFlow.Add(new EnemyDeathScytheAAltState(this));
        stateFlow.Add(new EnemyDeathScytheBAltState(this));
        stateFlow.Add(new EnemyDeathQuillFlurryState(this));
        stateFlow.Add(new EnemyDeathHeavyCastState(this));

        stateFlow.Add(new EnemyDeathDeadzonesState(this));
        stateFlow.Add(new EnemyDeathScytheBAltState(this));
        stateFlow.Add(new EnemyDeathQuillFlurryState(this));
        stateFlow.Add(new EnemyDeathScytheAState(this));
        stateFlow.Add(new EnemyDeathHeavyCastState(this));

        stateFlow.Add(new EnemyDeathDeadzonesState(this));
        stateFlow.Add(new EnemyDeathQuillFlurryState(this));
        stateFlow.Add(new EnemyDeathScytheAState(this));
        stateFlow.Add(new EnemyDeathScytheBState(this));
        stateFlow.Add(new EnemyDeathHeavyCastState(this));
    }

    public void ResetFlow()
    {
        flowIndex = 0;
        quillIndex = 0;
        stateAnimationTimes = new Stack<float>();
        stateDurationTimes = new Stack<float>();

        deathVFX.PlayEffect();
        deathSpell.EnableKillBox(false);

        foreach (BulletStateMachine bullet in deathQuills)
        {
            ResetQuill(bullet);
        }

        foreach (DeathDeadzone deadzone in deathDeadzones)
        {
            deadzone.ResetZone();
        }
    }

    public void IncrementFlow()
    {
        int newFlow = flowIndex + 1;

        if (newFlow <= stateFlow.Count)
        {
            flowIndex = newFlow;
        }
    }

    public void DecrementFlow()
    {
        if (flowIndex <= 0)
        {
            return;
        }

        flowIndex--;
    }

    public void AddAnimationTime(float time)
    {
        stateAnimationTimes.Push(time);
    }

    public void AddDurationTime(float time)
    {
        stateDurationTimes.Push(time);
    }

    public float GetAnimationTime()
    {
        return stateAnimationTimes.Pop();
    }

    public float GetDurationTime()
    {
        return stateDurationTimes.Pop();
    }

    public Transform GetRestPosition()
    {
        return restPosition;
    }

    public Transform GetScytheAPosition()
    {
        return scytheAPosition;
    }

    public Transform GetScytheBPosition()
    {
        return scytheBPosition;
    }

    public State GetNextState()
    {
        int index = Mathf.Clamp(flowIndex, 0, stateFlow.Count - 1);

        return stateFlow[index];
    }

    public DeathScythe GetWeapon()
    {
        return deathScythe;
    }

    public FingerOfDeath GetSpell()
    {
        return deathSpell;
    }

    public AudioClip[] GetDeathLines()
    {
        return deathBossVoiceLines;
    }

    public string[] GetDeathLineText()
    {
        return deathBossLineText;
    }

    public AudioSource GetDeathAudioSource()
    {
        return deathSource;
    }

    public DeathDeadzone GetDeathDeadzone()
    {
        DeathDeadzone deadzone = deathDeadzones[deadZoneIndex];
        deadZoneIndex++;
        return deadzone;
    }

    public BulletStateMachine GetQuill()
    {
        if (quillIndex >= deathQuills.Length)
        {
            return null;
        }

        BulletStateMachine availableQuill = deathQuills[quillIndex];
        quillIndex++;

        StartReaction.ReactionStarted(this);

        return availableQuill;
    }

    private void ReplaceQuill()
    {
        quillIndex--;
    }

    public void SetQuillAtFiringPoint(BulletStateMachine quill, int location)
    {
        Bullet bullet = quill.GetComponent<Bullet>();
        bullet.SetFiringPosition(quillPositions[location]);
    }

    public void ResetQuill(BulletStateMachine quill)
    {
        Bullet bullet = quill.GetComponent<Bullet>();
        bullet.ResetBullet(quillPositions[0]);
    }

    public void ToggleShield(bool toggle)
    {
        int index = flowIndex / 5;

        if (index > 2)
        {
            return;
        }

        deathShields[index].gameObject.SetActive(toggle);
    }

    public void ToggleWeakPoint(bool toggle)
    {
        weakPoint.SetActive(toggle);
    }

    public int GetFlowIndex()
    {
        return flowIndex;
    }

    public bool GetIsOutOfMoves()
    {
        return flowIndex > 12;
    }

    public DeathHealth GetHealth()
    {
        return health;
    }

    public RewindState GetRewindState()
    {
        return rewindState;
    }

    public LockOnTarget GetRevenantTarget()
    {
        return revenantTarget;
    }

    public void UndoReaction()
    {
        ReplaceQuill();
    }
}
