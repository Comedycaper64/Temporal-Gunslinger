using UnityEngine;

public class RewindableAnimator : RewindableMovement
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private float animationSpeedModifier = 1f;

    [SerializeField]
    private bool dirtySwordFix;

    private void Start()
    {
        ToggleMovement(true);

        if (dirtySwordFix)
        {
            BeginPlay();
        }
    }

    private void Update()
    {
        float animSpeed = Mathf.Clamp(GetSpeed() * animationSpeedModifier, -1f, 1f);
        animator.SetFloat("animSpeed", animSpeed);
    }
}
