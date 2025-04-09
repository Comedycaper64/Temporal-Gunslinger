using UnityEngine;

public class TrickshotCoin : MonoBehaviour
{
    [SerializeField]
    private RewindableAnimator rewindableAnimator;

    public void SetCoin(Vector3 newPosition)
    {
        transform.position = newPosition;
        transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);

        rewindableAnimator.BeginPlay();
    }
}
