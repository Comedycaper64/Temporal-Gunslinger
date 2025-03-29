using System.Collections;
using UnityEngine;

public class CinematicSkipVFXCleanup : MonoBehaviour
{
    private Coroutine skipCoroutine;

    [SerializeField]
    private VFXPlayback[] visualEffects;

    [SerializeField]
    private GameObject[] objectsToDisable;

    [SerializeField]
    private GameObject[] objectsToEnable;

    private void OnEnable()
    {
        PauseMenuUI.OnSkipCutscene += VFXCleanup;
    }

    private void OnDisable()
    {
        PauseMenuUI.OnSkipCutscene -= VFXCleanup;

        if (skipCoroutine != null)
        {
            StopCoroutine(skipCoroutine);
        }
    }

    private void VFXCleanup()
    {
        skipCoroutine = StartCoroutine(DelayedCleanup());
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
        if (objectsToEnable.Length > 0)
        {
            foreach (GameObject obj in objectsToEnable)
            {
                Debug.Log("Setting active: " + obj.name);
                obj.SetActive(true);
            }
        }
    }
}
