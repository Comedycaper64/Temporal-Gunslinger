using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingHallway : MonoBehaviour
{
    private bool shouldScroll = false;

    [SerializeField]
    private float scrollSpeed = 5f;
    private float teleportDistance = 40f;

    [SerializeField]
    private Transform[] hallways;

    [SerializeField]
    private Transform[] otherSceneObjects;

    [SerializeField]
    private Transform[] thingsToDisable;

    private void Start()
    {
        ToggleScroll(false);
    }

    private void Update()
    {
        if (!shouldScroll)
        {
            return;
        }

        MoveHallways();
    }

    private void MoveHallways()
    {
        foreach (Transform hallway in hallways)
        {
            hallway.position += Vector3.back * scrollSpeed * Time.deltaTime;
            if ((transform.position.z - hallway.position.z) > teleportDistance)
            {
                hallway.position += new Vector3(0, 0, 20f * hallways.Length);
            }
        }
    }

    public void ToggleScroll(bool toggle)
    {
        shouldScroll = toggle;
        foreach (Transform hallway in hallways)
        {
            hallway.gameObject.SetActive(toggle);
        }
        foreach (Transform sceneObject in otherSceneObjects)
        {
            sceneObject.gameObject.SetActive(toggle);
        }

        if (toggle)
        {
            foreach (Transform disableObject in thingsToDisable)
            {
                disableObject.gameObject.SetActive(false);
            }
        }
    }
}
