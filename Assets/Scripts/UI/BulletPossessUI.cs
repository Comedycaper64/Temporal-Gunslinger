using UnityEngine;

public class BulletPossessUI : MonoBehaviour
{
    private bool bCanPossess;
    private Transform possessTarget;

    [SerializeField]
    private CanvasGroupFader possessUI;

    private Vector3 uiOffset = new Vector3(0, 0.1f, 0);

    private void OnEnable()
    {
        BulletPossessor.OnNewCentralPossessable += SetUITarget;
        GameManager.OnGameStateChange += DisableUI;
        possessUI.SetCanvasGroupAlpha(0f);
    }

    private void OnDisable()
    {
        BulletPossessor.OnNewCentralPossessable -= SetUITarget;
        GameManager.OnGameStateChange -= DisableUI;
    }

    private void Update()
    {
        if (possessTarget)
        {
            Vector3 viewPos = Camera.main.WorldToScreenPoint(possessTarget.position + uiOffset);
            possessUI.transform.position = viewPos;
        }
    }

    private void SetUITarget(object sender, BulletPossessTarget e)
    {
        if (!bCanPossess)
        {
            return;
        }

        if (!e)
        {
            //possessUI.gameObject.SetActive(false);
            possessUI.ToggleFade(false);
        }
        else
        {
            //possessUI.gameObject.SetActive(true);
            possessUI.ToggleFade(true);
            possessTarget = e.transform;
        }
    }

    private void DisableUI(object sender, StateEnum state)
    {
        if (state == StateEnum.inactive)
        {
            bCanPossess = false;
            possessUI.ToggleFade(false);
        }
        else
        {
            bCanPossess = true;
        }
    }
}
