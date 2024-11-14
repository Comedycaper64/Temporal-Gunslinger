using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBulletSpawner : RewindableMovement
{
    private int spawnIndex = 0;
    private float spawnTimer = 0f;

    [SerializeField]
    private float spawnTime;

    [SerializeField]
    private BulletStateMachine[] availableBullets;

    [SerializeField]
    private Transform spawnLocation;

    private void Update()
    {
        if (movementActive)
        {
            spawnTimer += GetSpeed() * Time.deltaTime;

            //Debug.Log(spawnTimer);

            if (spawnTimer > spawnTime)
            {
                spawnTimer = 0f;

                if (spawnIndex < availableBullets.Length)
                {
                    //Debug.Log("Locust Spawn");
                    availableBullets[spawnIndex].SwitchToActive();
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

        foreach (BulletStateMachine bullet in availableBullets)
        {
            if (!spawnLocation)
            {
                bullet.transform.position = transform.position;
            }
            else
            {
                bullet.transform.position = spawnLocation.position;
            }
        }
    }
}
