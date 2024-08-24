using System;
using UnityEngine;

public class FlameCarrier : MonoBehaviour, IReactable
{
    private DissolveController dissolver;

    [SerializeField]
    private GameObject fallingFireObject;
    private IFireStarter fireStarter;

    private IFireStarter thisFireStarter;

    [SerializeField]
    private VFXPlayback flameVFX;

    private void Awake()
    {
        dissolver = GetComponent<DissolveController>();
        fireStarter = fallingFireObject.GetComponent<IFireStarter>();
        thisFireStarter = GetComponent<IFireStarter>();
    }

    private void OnEnable()
    {
        if (fireStarter != null)
        {
            fireStarter.OnFireStarted += StartFlame;
        }
    }

    private void OnDisable()
    {
        if (fireStarter != null)
        {
            fireStarter.OnFireStarted -= StartFlame;
        }
    }

    public void StartFlame(object sender, EventArgs e)
    {
        Debug.Log("Flame started");

        flameVFX.PlayEffect();
        dissolver.StartDissolve(0.25f);

        if (thisFireStarter != null)
        {
            thisFireStarter.SetIsAflame(true);
        }

        StartReaction.ReactionStarted(this);
    }

    public void UndoReaction()
    {
        flameVFX.StopEffect();
        dissolver.StopDissolve();

        if (thisFireStarter != null)
        {
            thisFireStarter.SetIsAflame(false);
        }
    }
}
