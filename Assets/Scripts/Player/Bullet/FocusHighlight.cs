using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private GameObject targetHighlight;

    public void ToggleHighlight(bool toggle)
    {
        targetHighlight.SetActive(toggle);
    }
}
