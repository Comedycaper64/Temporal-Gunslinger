using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class AbilityCutInTextUI : MonoBehaviour
{
    private float fadeDelayTime = 2.5f;
    private CanvasGroupFader fader;
    private Coroutine delayedFadeCoroutine;

    [SerializeField]
    private TextMeshProUGUI cutInText;

    [SerializeField]
    private Color[] cutInColours;

    private void OnEnable()
    {
        fader = GetComponent<CanvasGroupFader>();
        fader.SetCanvasGroupAlpha(0f);

        PlayerConquestAbility.OnConquestAbility += OnAbility;
        PlayerConquestAbility.OnConquestAbilityText += OnText;

        PlayerFamineAbility.OnFamineAbility += OnAbility;
        PlayerFamineAbility.OnFamineAbilityText += OnText;

        PlayerPestilenceAbility.OnPestilenceAbility += OnAbility;
        PlayerPestilenceAbility.OnPestilenceAbilityText += OnText;

        EnemyDeathHeavyCastInterruptState.OnDeathInterrupted += DeathInterrupt;
        EnemyDeathHeavyCastInterruptState.OnDeathInterrupted += OnText;

        ReaperBossDialogue.OnReaperBossDialogue += DeathInterrupt;
        ReaperBossDialogue.OnReaperBossDialogue += OnText;
    }

    private void OnDisable()
    {
        PlayerConquestAbility.OnConquestAbility -= OnAbility;
        PlayerConquestAbility.OnConquestAbilityText -= OnText;

        PlayerFamineAbility.OnFamineAbility -= OnAbility;
        PlayerFamineAbility.OnFamineAbilityText -= OnText;

        PlayerPestilenceAbility.OnPestilenceAbility -= OnAbility;
        PlayerPestilenceAbility.OnPestilenceAbilityText -= OnText;

        EnemyDeathHeavyCastInterruptState.OnDeathInterrupted -= DeathInterrupt;
        EnemyDeathHeavyCastInterruptState.OnDeathInterrupted -= OnText;

        ReaperBossDialogue.OnReaperBossDialogue -= DeathInterrupt;
        ReaperBossDialogue.OnReaperBossDialogue -= OnText;

        if (delayedFadeCoroutine != null)
        {
            StopCoroutine(delayedFadeCoroutine);
        }
    }

    private void OnText(object sender, string text)
    {
        fader.SetCanvasGroupAlpha(0f);
        cutInText.text = text;
        fader.ToggleFade(true);

        delayedFadeCoroutine = StartCoroutine(DelayedFade());
    }

    private IEnumerator DelayedFade()
    {
        yield return new WaitForSecondsRealtime(fadeDelayTime);
        fader.ToggleFade(false);
    }

    private void DeathInterrupt(object sender, string e)
    {
        OnAbility(this, CutInType.Death);
    }

    private void OnAbility(object sender, CutInType cutIn)
    {
        if (cutIn == CutInType.Conquest)
        {
            cutInText.color = cutInColours[0];
        }
        else if (cutIn == CutInType.Famine)
        {
            cutInText.color = cutInColours[1];
        }
        else if (cutIn == CutInType.Pestilence)
        {
            cutInText.color = cutInColours[2];
        }
        else
        {
            cutInText.color = cutInColours[3];
        }
    }
}
