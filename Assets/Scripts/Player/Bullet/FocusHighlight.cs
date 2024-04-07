using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private GameObject targetHighlight;

    private void Start()
    {
        BulletPossessTarget.highlightables.Add(this);
    }

    private void OnDisable()
    {
        BulletPossessTarget.highlightables.Remove(this);
    }

    public void ToggleHighlight(bool toggle)
    {
        //Debug.Log("Toggle: " + toggle + gameObject);

        if (!targetHighlight)
        {
            Debug.Log("ERROR: highlight for " + gameObject.name + " not assigned");
            return;
        }

        targetHighlight.SetActive(toggle);
    }
}
