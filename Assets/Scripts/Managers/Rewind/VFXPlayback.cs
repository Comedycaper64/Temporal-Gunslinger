using UnityEngine;
using UnityEngine.VFX;

public class VFXPlayback : RewindableMovement
{
    // [SerializeField]
    // private float activePlayRateModifier;
    private float simulationTime = 0.0f;
    private bool effectPlaying;
    private VisualEffect visualEffect;
    private const string TIME_VARIABLE = "Time";
    private const string PLAYBACK_VARIABLE = "Playback";

    [SerializeField]
    private bool controlTimeVariable = false;

    [SerializeField]
    private bool controlPlaybackVariable = false;

    [SerializeField]
    private bool clamp01 = false;

    [SerializeField]
    private bool bPlayOnStart;

    private void Start()
    {
        visualEffect = GetComponent<VisualEffect>();

        // if (bPlayOnStart)
        // {
        //     PlayEffect();
        // }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        visualEffect = GetComponent<VisualEffect>();
        if (bPlayOnStart)
        {
            PlayEffect();
            BeginPlay();
        }
    }

    protected override float GetSpeed()
    {
        return speed * GetUnclampedTimescale();
    }

    private void Update()
    {
        if (!effectPlaying)
        {
            visualEffect.playRate = 1f;
            return;
        }

        if (controlPlaybackVariable)
        {
            visualEffect.SetFloat(PLAYBACK_VARIABLE, GetSpeed());
        }

        if (controlTimeVariable)
        {
            simulationTime += Time.deltaTime * GetSpeed();
            visualEffect.SetFloat(TIME_VARIABLE, simulationTime);
        }
        else
        {
            float speed = GetSpeed();
            if (clamp01)
            {
                speed = Mathf.Clamp01(speed);
            }

            visualEffect.playRate = speed;
        }
    }

    public void SetIsEffectPlaying(bool effectPlaying)
    {
        this.effectPlaying = effectPlaying;
    }

    public void StopEffect()
    {
        if (!visualEffect)
        {
            return;
        }

        visualEffect.Reinit();
        visualEffect.Stop();
        effectPlaying = false;
        simulationTime = 0.0f;
        ToggleMovement(false);
    }

    public void PlayEffect()
    {
        if (!visualEffect)
        {
            return;
        }

        visualEffect.Play();
        ToggleMovement(true);
        effectPlaying = true;

        if (controlTimeVariable)
        {
            simulationTime = 0.0f;
            VFXPlayed.VFXPlay(this);
        }
    }
}
