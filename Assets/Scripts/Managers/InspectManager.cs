using System;
using System.Collections.Generic;
using UnityEngine;

public class InspectManager : MonoBehaviour
{
    private bool bInspecting = false;
    private bool bCanInspect = false;
    private InspectTarget targetInspectable;

    [SerializeField]
    private GameObject inspectablesObject;
    private List<InspectTarget> sceneInspectables = new List<InspectTarget>();

    public static EventHandler<InspectTarget> OnInspect;
    public static EventHandler<InspectTarget> OnCanInspect;

    private void Start()
    {
        GameManager.OnGameStateChange += ToggleCanInspect;
        //RewindManager.OnRewindToStart += TurnOnInspect;
        InputManager.Instance.OnPossessAction += TryInspect;

        if (inspectablesObject)
        {
            InspectTarget[] sceneTargets =
                inspectablesObject.GetComponentsInChildren<InspectTarget>();
            foreach (InspectTarget target in sceneTargets)
            {
                sceneInspectables.Add(target);
            }
        }
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= ToggleCanInspect;
        //RewindManager.OnRewindToStart -= TurnOnInspect;
        InputManager.Instance.OnPossessAction -= TryInspect;
    }

    private void Update()
    {
        if (bCanInspect && !bInspecting)
        {
            //look for inspectables
            //when targeting a new inspectable, fire off an OnCanInspect
            if (sceneInspectables.Count == 0)
            {
                return;
            }

            FindInspectables();
        }
    }

    private void FindInspectables()
    {
        InspectTarget closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        foreach (InspectTarget target in sceneInspectables)
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(target.transform.position);

            //Debug.Log(viewPos);

            if (
                viewPos.z < 0f
                || viewPos.x < 0.4f
                || viewPos.x > 0.6f
                || viewPos.y < 0.4f
                || viewPos.y > 0.6f
            )
            {
                continue;
            }

            Vector3 toCenter = viewPos - new Vector3(0.5f, 0.5f);
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        if (closestTarget == null)
        {
            if (targetInspectable != null)
            {
                targetInspectable = null;
                OnCanInspect?.Invoke(this, null);
            }
            return;
        }

        if (closestTarget != targetInspectable)
        {
            targetInspectable = closestTarget;
            OnCanInspect?.Invoke(this, targetInspectable);
        }
    }

    private void ToggleCanInspect(object sender, StateEnum e)
    {
        if (e == StateEnum.idle)
        {
            bCanInspect = true;
        }
        else
        {
            bCanInspect = false;
            bInspecting = false;
            OnCanInspect?.Invoke(this, null);
            OnInspect?.Invoke(this, null);
        }
    }

    private void TurnOnInspect()
    {
        bCanInspect = true;
    }

    public void TryInspect()
    {
        if (!bCanInspect)
        {
            return;
        }

        if (bInspecting)
        {
            bInspecting = false;
            OnInspect?.Invoke(this, null);
            return;
        }

        if (targetInspectable)
        {
            bInspecting = true;
            OnInspect?.Invoke(this, targetInspectable);
        }
    }
}
