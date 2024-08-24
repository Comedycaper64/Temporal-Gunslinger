using UnityEngine;

public class AnimatorSpeedRandomiser : MonoBehaviour
{
    [SerializeField]
    private float minRandomMult = 0f;

    [SerializeField]
    private float maxRandomMult = 1f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("random", Random.Range(minRandomMult, maxRandomMult));
    }
}
