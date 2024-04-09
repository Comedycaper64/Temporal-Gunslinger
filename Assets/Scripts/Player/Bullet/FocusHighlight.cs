using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private GameObject targetHighlight;

    // private void Start()
    // {
    //     BulletPossessTarget.highlightables.Add(this);
    //     Debug.Log("Added: " + name);
    // }

    private void OnEnable()
    {
        BulletPossessTarget.highlightables.Add(this);
        //Debug.Log("Added: " + name);
    }

    private void OnDisable()
    {
        BulletPossessTarget.highlightables.Remove(this);
        //Debug.Log("Removed: " + name);
    }

    public void ToggleHighlight(bool toggle)
    {
        //Debug.Log("Toggle: " + toggle + gameObject);

        if (!targetHighlight)
        {
            // Debug.Log("ayaya");
            Debug.Log("ERROR: highlight for " + gameObject.name + " not assigned");
            return;
        }

        targetHighlight.SetActive(toggle);
    }
}
