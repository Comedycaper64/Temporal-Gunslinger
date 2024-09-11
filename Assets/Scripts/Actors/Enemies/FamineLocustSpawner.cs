using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FamineLocustSpawner : RewindableMovement
{
    private int spawnIndex = 0;
    private float spawnTimer = 0f;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private BulletStateMachine[] availableLocusts;

    private void Update()
    {
        if (movementActive)
        {
            spawnTimer += GetSpeed() * Time.deltaTime;

            //Debug.Log(spawnTimer);

            if (spawnTimer > spawnTime)
            {
                spawnTimer = 0f;

                if (spawnIndex < availableLocusts.Length)
                {
                    Debug.Log("Locust Spawn");
                    availableLocusts[spawnIndex].SwitchToActive();
                }

                spawnIndex++;
            }
            else if (spawnTimer < 0f)
            {
                spawnTimer = spawnTime;
                spawnIndex--;
            }
        }
    }

    public void ResetSpawner()
    {
        spawnIndex = 0;
        spawnTimer = 0f;

        foreach (BulletStateMachine locust in availableLocusts)
        {
            locust.transform.position = transform.position;
        }
    }
}
