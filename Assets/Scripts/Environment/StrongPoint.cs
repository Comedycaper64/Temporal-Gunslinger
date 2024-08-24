using UnityEngine;

public class StrongPoint : MonoBehaviour, IDamageable
{
    [SerializeField]
    private float velocityConservation = 0.8f;

    public void ProjectileHit(out float velocityConservation)
    {
        velocityConservation = this.velocityConservation;
    }
}
