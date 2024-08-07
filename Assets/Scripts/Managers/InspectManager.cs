using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectManager : MonoBehaviour
{
    private bool bCanInspect = false;
    private InspectTarget targetInspectable;

    public static EventHandler<InspectTarget> OnInspect;
    public static EventHandler<bool> OnCanInspect;

    private void Start()
    {
        GameManager.OnGameStateChange += ToggleInspect;
        InputManager.Instance.OnPossessAction += TryInspect;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleInspect;
        InputManager.Instance.OnPossessAction -= TryInspect;
    }

    private void Update()
    {
        if (bCanInspect)
        {
            //look for inspectables
            //when targeting a new inspectable, fire off an OnCanInspect
        }
    }

    private void ToggleInspect(object sender, StateEnum e)
    {
        if (e == StateEnum.idle)
        {
            bCanInspect = true;
        }
        else
        {
            bCanInspect = false;
            OnCanInspect?.Invoke(this, false);
            OnInspect?.Invoke(this, null);
        }
    }

    public void TryInspect()
    {
        // if has inspect target
        OnInspect?.Invoke(this, targetInspectable);
    }
}
