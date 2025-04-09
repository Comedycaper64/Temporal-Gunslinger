using UnityEngine;

public class RicochetManager : MonoBehaviour, IReactable
{
    private static int ricochetIndex = 0;
    private static int impactIndex = 0;

    [SerializeField]
    private VFXPlayback[] localRicochetVFX;
    private static VFXPlayback[] ricochetVFX;

    [SerializeField]
    private ImpactEffect[] localImpactEffects;
    private static ImpactEffect[] impactEffects;

    private static RicochetManager ricochetManager;

    private void OnEnable()
    {
        ricochetManager = this;
        ricochetVFX = localRicochetVFX;
        impactEffects = localImpactEffects;

        ricochetIndex = 0;
        impactIndex = 0;
    }

    public static void SpawnRicochetVFX(Transform bulletTransform, Quaternion vfxRotation)
    {
        ricochetVFX[ricochetIndex].transform.SetPositionAndRotation(
            bulletTransform.position,
            vfxRotation
        );
        ricochetVFX[ricochetIndex].gameObject.SetActive(true);

        ricochetIndex++;

        if (ricochetIndex >= ricochetVFX.Length)
        {
            ricochetIndex = 0;
        }

        StartReaction.ReactionStarted(ricochetManager);
    }

    public static void SpawnRicochetImpact(Collision ricochetCollision)
    {
        Vector3 impactPoint = ricochetCollision.GetContact(0).point;
        Vector3 impactNormal = ricochetCollision.GetContact(0).normal;

        impactEffects[impactIndex].transform.position =
            impactPoint + 0.1f * impactNormal.normalized;
        impactEffects[impactIndex].transform.rotation = Quaternion.LookRotation(-impactNormal);
        impactEffects[impactIndex].gameObject.SetActive(true);
        impactEffects[impactIndex].ResetEffect();
        impactEffects[impactIndex].ToggleMovement(true);

        impactIndex++;

        if (impactIndex >= impactEffects.Length)
        {
            impactIndex = 0;
        }
    }

    public void UndoReaction()
    {
        ricochetIndex--;

        if (ricochetIndex < 0)
        {
            ricochetIndex = ricochetVFX.Length - 1;
        }

        ricochetVFX[ricochetIndex].StopEffect();
        ricochetVFX[ricochetIndex].gameObject.SetActive(false);
    }
}
