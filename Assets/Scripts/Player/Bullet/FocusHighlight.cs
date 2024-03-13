using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private GameObject targetHighlight;

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
