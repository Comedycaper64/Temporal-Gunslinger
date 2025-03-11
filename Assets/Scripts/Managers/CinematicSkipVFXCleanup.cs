using System.Collections;
using UnityEngine;

public class CinematicSkipVFXCleanup : MonoBehaviour
{
    [SerializeField]
    private VFXPlayback[] visualEffects;

    [SerializeField]
    private GameObject[] objectsToDisable;

    private void OnEnable()
    {
        PauseMenuUI.OnSkipCutscene += VFXCleanup;
    }

    private void OnDisable()
    {
        PauseMenuUI.OnSkipCutscene -= VFXCleanup;
    }

    private void VFXCleanup()
    {
        StartCoroutine(DelayedCleanup());
    }

    private IEnumerator DelayedCleanup()
    {
        yield return new WaitForSeconds(.1f);

        foreach (VFXPlayback vfx in visualEffects)
        {
            vfx.PlayEffect();
        }

        if (objectsToDisable.Length > 0)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
        }
    }
}
