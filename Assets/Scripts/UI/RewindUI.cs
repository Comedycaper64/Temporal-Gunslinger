using UnityEngine;

public class RewindUI : MonoBehaviour
{
    private float clockHandTurnSpeed = 50f;
    private CanvasGroupFader rewindFader;

    [SerializeField]
    private Transform clockhandTransform;

    private void Start()
    {
        rewindFader = GetComponent<CanvasGroupFader>();
        RewindManager.OnRewindToggle += ToggleRewindUI;
    }

    private void OnDisable()
    {
        RewindManager.OnRewindToggle -= ToggleRewindUI;
    }

    private void Update()
    {
        clockhandTransform.eulerAngles += new Vector3(0, 0, clockHandTurnSpeed * Time.deltaTime);
    }

    private void ToggleRewindUI(object sender, bool e)
    {
        rewindFader.ToggleFade(e);
    }
}
