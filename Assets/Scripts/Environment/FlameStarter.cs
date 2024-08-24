using System;
using UnityEngine;

public class FlameStarter : MonoBehaviour, IFireStarter
{
    [SerializeField]
    private WeakPoint weakPoint;

    public event EventHandler OnFireStarted;

    private void OnEnable()
    {
        weakPoint.OnCrush += StartFires;
    }

    private void OnDisable()
    {
        weakPoint.OnCrush -= StartFires;
    }

    private void StartFires(object sender, EventArgs e)
    {
        OnFireStarted?.Invoke(this, null);
    }

    public bool GetIsAflame()
    {
        return true;
    }

    public void SetIsAflame(bool aflame) { }
}
