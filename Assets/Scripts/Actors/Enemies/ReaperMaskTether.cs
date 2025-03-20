using UnityEngine;

public class ReaperMaskTether : MonoBehaviour
{
    private MentalLinkTether tether;

    [SerializeField]
    private Transform startPoint;

    [SerializeField]
    private Transform endPoint;

    private void Awake()
    {
        tether = GetComponent<MentalLinkTether>();
    }

    private void Update()
    {
        tether.SetTetherPoint(startPoint.position, endPoint.position);
    }
}
