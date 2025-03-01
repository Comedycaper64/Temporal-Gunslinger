using System;
using UnityEngine;

public class TimedDanger : RewindableMovement
{
    [SerializeField]
    private float timeToKill;
    private float timeOffset = 0f;

    [SerializeField]
    private Sprite pocketwatchDangerSprite;

    private ISetAttacker attacker;

    public static Action OnTimedDangerChange;

    protected override void OnEnable()
    {
        base.OnEnable();
        attacker = GetComponent<ISetAttacker>();
        attacker.OnAttackToggled += SetDanger;
        attacker.OnTimeOffset += SetOffset;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        attacker.OnAttackToggled -= SetDanger;
        attacker.OnTimeOffset -= SetOffset;
        DangerTracker.dangers.Remove(this);
    }

    private void SetDanger(object sender, bool toggle)
    {
        float deathTime = -1;

        if (toggle)
        {
            deathTime = timeToKill;
            DangerTracker.dangers.Add(
                this,
                new PocketwatchDanger(
                    pocketwatchDangerSprite,
                    PocketwatchUI.pocketwatchTime + deathTime + timeOffset
                )
            );
            OnTimedDangerChange?.Invoke();
        }
        else
        {
            DangerTracker.dangers.Remove(this);
            OnTimedDangerChange?.Invoke();
        }
    }

    private void SetOffset(object sender, float offset)
    {
        timeOffset = offset;
    }
}
