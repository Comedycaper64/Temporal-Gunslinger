using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentDamager : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<WeakPoint>(out WeakPoint weakPoint))
        {
            weakPoint.EnvironmentCrush();
        }
    }
}
