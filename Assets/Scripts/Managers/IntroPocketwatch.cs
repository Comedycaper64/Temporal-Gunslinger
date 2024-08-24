using UnityEngine;
using MoreMountains.Feedbacks;

public class IntroPocketwatch : MonoBehaviour
{
    private bool lookAtCamera;
    private Vector3 refVelocity;
    private MMF_Player player;

    [SerializeField]
    private TutorialGameManager tutorialGameManager;

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
                lookAtCamera = false;
                tutorialGameManager.StartGame();
            }
        }
    }

    public void StopFeedbackPlayer()
    {
        //player.PauseFeedbacks();
        player.StopFeedbacks();
        lookAtCamera = true;
    }
}
