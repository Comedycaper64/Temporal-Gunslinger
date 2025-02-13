using UnityEngine;

public class FutureShield : MonoBehaviour, IReactable
{
    [SerializeField]
    private DissolveController dissolveController;

    [SerializeField]
    private float shieldBreakThreshold = 500f;

    [SerializeField]
    private BreakablePoint breakablePoint;

    [SerializeField]
    private GameObject shieldCollider;

    private void OnEnable()
    {
        breakablePoint.OnHit += TestBulletSpeed;
    }

    private void OnDisable()
    {
        breakablePoint.OnHit -= TestBulletSpeed;
    }

    private void BreakShield()
    {
        breakablePoint.gameObject.SetActive(false);
        shieldCollider.gameObject.SetActive(false);
        dissolveController.StartDissolve();

        StartReaction.ReactionStarted(this);
    }

    private void TestBulletSpeed(object sender, float speed)
    {
        if (speed > shieldBreakThreshold)
        {
            BreakShield();
        }
    }

    public void UndoReaction()
    {
        breakablePoint.gameObject.SetActive(true);
        shieldCollider.gameObject.SetActive(true);
        dissolveController.StopDissolve();
    }
}
