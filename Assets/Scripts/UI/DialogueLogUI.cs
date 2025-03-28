using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public struct DialogueLog
{
    public DialogueLog(ActorSO actorSO, string logText)
    {
        logActor = actorSO;
        this.logText = logText;
    }

    public ActorSO logActor;
    public string logText;
}

public class DialogueLogUI : MonoBehaviour
{
    private bool logUIActive = false;
    private bool listeningToInput = false;
    private int logQueueMax = 10;
    private static Queue<DialogueLog> logQueue = new Queue<DialogueLog>();

    [SerializeField]
    private DialogueUI dialogueUI;

    [SerializeField]
    private DialogueLogPanelUI[] logPanels;

    [SerializeField]
    private CanvasGroupFader dialogueLogFader;

    [SerializeField]
    private ScrollRect scrollRect;

    public static EventHandler<bool> OnLogToggle;

    private void Awake()
    {
        logQueueMax = logPanels.Length;

        dialogueLogFader.SetCanvasGroupAlpha(0f);
        dialogueLogFader.ToggleBlockRaycasts(false);
        DisableLogPanels();
    }

    private void OnEnable()
    {
        dialogueUI.OnNewDialogue += AddDialogueLog;
        DialogueManager.OnToggleDialogueUI += ToggleInput;
    }

    private void OnDisable()
    {
        dialogueUI.OnNewDialogue -= AddDialogueLog;
        DialogueManager.OnToggleDialogueUI -= ToggleInput;
        ToggleInput(this, false);
    }

    private void RefreshLogPanels()
    {
        Queue<DialogueLog> revQueue = new Queue<DialogueLog>(logQueue.Reverse());
        revQueue.Dequeue();

        DialogueLog[] currentLogs = revQueue.ToArray();
        int logIndex = 0;
        DisableLogPanels();

        foreach (DialogueLog log in currentLogs)
        {
            DialogueLogPanelUI logPanel = logPanels[logIndex];
            logPanel.gameObject.SetActive(true);
            logPanel.SetPanel(log);

            logIndex++;
        }
        scrollRect.verticalNormalizedPosition = 0f;
    }

    private void DisableLogPanels()
    {
        foreach (DialogueLogPanelUI log in logPanels)
        {
            log.gameObject.SetActive(false);
        }
    }

    private void ToggleLogUI()
    {
        logUIActive = !logUIActive;
        if (logUIActive)
        {
            RefreshLogPanels();
        }

        dialogueLogFader.ToggleBlockRaycasts(logUIActive);
        dialogueLogFader.ToggleFade(logUIActive);

        OnLogToggle?.Invoke(this, logUIActive);
    }

    private void AddDialogueLog(object sender, DialogueLog log)
    {
        if (logQueue.Count >= logQueueMax)
        {
            logQueue.Dequeue();
        }

        logQueue.Enqueue(log);
    }

    private void ToggleInput(object sender, bool toggle)
    {
        if (toggle)
        {
            if (listeningToInput)
            {
                return;
            }

            InputManager.Instance.OnRewindAction += ToggleLogUI;
            listeningToInput = true;
        }
        else
        {
            InputManager.Instance.OnRewindAction -= ToggleLogUI;
            listeningToInput = false;
        }
    }
}
