using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private GameObject targetHighlight;

    private void OnEnable()
    {
        BulletPossessTarget.highlightables.Add(this);
        ToggleHighlight(false);
    }

    private void OnDisable()
    {
        BulletPossessTarget.highlightables.Remove(this);
    }

    public void ToggleHighlight(bool toggle)
    {
        if (!targetHighlight)
        {
            Debug.Log("ERROR: highlight for " + gameObject.name + " not assigned");
            return;
        }

        targetHighlight.SetActive(toggle);
    }
}
