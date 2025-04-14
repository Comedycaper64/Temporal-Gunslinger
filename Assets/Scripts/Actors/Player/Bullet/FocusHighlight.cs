using UnityEngine;

public class FocusHighlight : MonoBehaviour, IHighlightable
{
    [SerializeField]
    private GameObject targetHighlight;

    private void OnEnable()
    {
        BulletPossessTarget.highlightables.Add(this);
        ToggleHighlight(BulletPossessTarget.highlightActive);
    }

    private void OnDisable()
    {
        BulletPossessTarget.highlightables.Remove(this);
        ToggleHighlight(false);
    }

    public void ToggleHighlight(bool toggle)
    {
        if (!targetHighlight)
        {
            //Debug.Log("ERROR: highlight for " + gameObject.name + " not assigned");
            return;
        }

        targetHighlight.SetActive(toggle);
    }
}
