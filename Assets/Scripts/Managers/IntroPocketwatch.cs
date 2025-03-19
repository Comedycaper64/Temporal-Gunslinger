using UnityEngine;
using MoreMountains.Feedbacks;

public class IntroPocketwatch : MonoBehaviour
{
    private bool lookAtCamera;
    private Vector3 refVelocity;
    private MMF_Player player;

    [SerializeField]
    private AudioManager audioManager;

    [SerializeField]
    private AudioClip introTrack;

    [SerializeField]
    private TutorialGameManager tutorialGameManager;

    [SerializeField]
    private DefianceGameManager defianceGameManager;

    private void Awake()
    {
        player = GetComponent<MMF_Player>();
    }

    private void Update()
    {
        if (lookAtCamera)
        {
            transform.eulerAngles = Vector3.SmoothDamp(
                transform.eulerAngles,
                Vector3.zero,
                ref refVelocity,
                0.5f
            );

            if (Vector3.Distance(transform.eulerAngles, Vector3.zero) < 1f)
            {
                StartGame();
            }
        }
    }

    public void StopFeedbackPlayer()
    {
        //player.PauseFeedbacks();
        player.StopFeedbacks();
        lookAtCamera = true;
        audioManager.FadeOutMusic();
    }

    public void StartGame()
    {
        lookAtCamera = false;

        if (!tutorialGameManager)
        {
            defianceGameManager.StartGame();
        }
        else
        {
            tutorialGameManager.StartGame();
        }

        audioManager.SetMusicTrack(introTrack, null);
        audioManager.FadeInMusic();
    }
}
