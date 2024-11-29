using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatingBulletSpawner : RewindableMovement
{
    private int spawnIndex = 0;
    private float spawnTimer = 0f;

    [SerializeField]
    private bool initialBulletLaunch = false;

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

    public float GetSpawnTimer()
    {
        return spawnTimer;
    }

    public float GetSpawnTime()
    {
        return spawnTime;
    }

    public override void ToggleMovement(bool toggle)
    {
        base.ToggleMovement(toggle);

        if (toggle && initialBulletLaunch && (spawnIndex == 0))
        {
            availableBullets[spawnIndex].SwitchToActive();
            spawnIndex++;
        }
    }
}
