using UnityEngine;

public class ScrollingHallway : MonoBehaviour
{
    private bool shouldScroll = false;

    [SerializeField]
    private float scrollSpeed = 5f;

    [SerializeField]
    private float teleportDistance = 40f;

    [SerializeField]
    private float hallwayLength = 20f;

    [SerializeField]
    private Vector3 hallwayMovementDirection = Vector3.back;

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
            hallway.position += hallwayMovementDirection * scrollSpeed * Time.deltaTime;
            if (
                (transform.position - hallway.position).sqrMagnitude
                > Mathf.Pow(teleportDistance, 2)
            )
            {
                //hallway.position += new Vector3(0, 0, 20f * hallways.Length);
                hallway.position += -hallwayMovementDirection * hallwayLength * hallways.Length;
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

        // foreach (Transform sceneObject in otherSceneObjects)
        // {
        //     sceneObject.gameObject.SetActive(toggle);
        // }

        for (int i = 0; i < otherSceneObjects.Length; i++)
        {
            otherSceneObjects[i].gameObject.SetActive(toggle);
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
