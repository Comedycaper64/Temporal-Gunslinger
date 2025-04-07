using System;
using UnityEngine;

public class ReaperBossDialogue : MonoBehaviour
{
    private int lineIndex;

    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip[] dialogueLines;

    [SerializeField]
    private string[] dialogueLineTexts;

    [SerializeField]
    private WeakPoint[] weakPoints;

    public static EventHandler<string> OnReaperBossDialogue;

    private void OnEnable()
    {
        lineIndex = 0;
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.OnHit += PlayDialogue;
        }
    }

    private void OnDisable()
    {
        foreach (WeakPoint weakPoint in weakPoints)
        {
            weakPoint.OnHit -= PlayDialogue;
        }
    }

    private void PlayDialogue(object sender, EventArgs e)
    {
        if (lineIndex >= dialogueLines.Length)
        {
            return;
        }

        source.Stop();
        source.clip = dialogueLines[lineIndex];
        source.Play();

        OnReaperBossDialogue?.Invoke(this, dialogueLineTexts[lineIndex]);

        lineIndex++;
    }
}
