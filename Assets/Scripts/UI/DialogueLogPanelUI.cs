using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueLogPanelUI : MonoBehaviour
{
    private Material actorFontMaterialInstance;

    [SerializeField]
    private Image panel;

    [SerializeField]
    private TextMeshProUGUI actorText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private void Awake()
    {
        actorFontMaterialInstance = actorText.fontMaterial;
    }

    public void SetPanel(DialogueLog log)
    {
        panel.gameObject.SetActive(true);
        ActorSO actor = log.logActor;

        panel.color = actor.GetActorNameColour();
        actorText.text = actor.GetActorName();
        actorFontMaterialInstance.SetColor("_UnderlayColor", actor.GetActorNameColour());

        dialogueText.text = log.logText;
    }
}
