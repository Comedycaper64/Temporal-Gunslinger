using System;
using UnityEngine;

public class EnvironmentDamager : MonoBehaviour
{
    [SerializeField]
    private bool bIsFragile;

    [SerializeField]
    private RewindState rewindState;
    public event Action OnCrush;

    private void Awake()
    {
        rewindState.ToggleMovement(true);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (rewindState.GetTimeSpeed() < 0f)
        {
            return;
        }

        if (other.gameObject.TryGetComponent<WeakPoint>(out WeakPoint weakPoint))
        {
            weakPoint.EnvironmentCrush();
            if (bIsFragile)
            {
                OnCrush?.Invoke();
            }
        }
        else if (bIsFragile && other.gameObject.GetComponent<StrongPoint>())
        {
            OnCrush?.Invoke();
        }
    }
}
