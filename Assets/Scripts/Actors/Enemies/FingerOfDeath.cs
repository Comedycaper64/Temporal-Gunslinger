using UnityEngine;

public class FingerOfDeath : MonoBehaviour
{
    [SerializeField]
    private Transform[] startPoints;

    [SerializeField]
    private Transform startEndPoint;
    private Transform endPoint;

    [SerializeField]
    private GameObject killBox;

    [SerializeField]
    private MentalLinkTether[] tethers;

    private void Start()
    {
        endPoint = GameManager.GetRevenant();
        DisableAttackVisual();
    }

    public void UpdateAttackVisual(float lerp)
    {
        Vector3 endPosition = Vector3.Lerp(startEndPoint.position, endPoint.position, lerp);

        for (int i = 0; i < tethers.Length; i++)
        {
            tethers[i].SetTetherPoint(startPoints[i].position, endPosition);
        }
    }

    public void DisableAttackVisual()
    {
        foreach (MentalLinkTether tether in tethers)
        {
            tether.SeverTetherPoint();
        }
    }

    public void EnableKillBox(bool toggle)
    {
        killBox.SetActive(toggle);
    }
}
