using UnityEngine;

public class RepeatingMaskSpawner : RewindableMovement
{
    private int spawnIndex = 0;
    private float spawnTimer = 0f;

    [SerializeField]
    private float[] spawnTimes;

    [SerializeField]
    private EnemyMaskBossStateMachine[] availableMasks;

    private void Update()
    {
        if (movementActive)
        {
            spawnTimer += GetSpeed() * Time.deltaTime;

            //Debug.Log(spawnTimer);

            if (spawnTimer > spawnTimes[spawnIndex])
            {
                spawnTimer = 0f;

                if (spawnIndex < availableMasks.Length)
                {
                    availableMasks[spawnIndex].SwitchToActiveState();
                }

                spawnIndex++;
            }
            else if (spawnTimer < 0f)
            {
                spawnIndex--;
                spawnTimer = spawnTimes[spawnIndex];
            }
        }
    }

    public void ResetSpawner()
    {
        spawnIndex = 0;
        spawnTimer = 0f;

        // foreach (BulletStateMachine bullet in availableMasks)
        // {
        //     if (!spawnLocation)
        //     {
        //         bullet.transform.position = transform.position;
        //     }
        //     else
        //     {
        //         bullet.transform.position = spawnLocation.position;
        //     }
        // }
    }

    // public float GetSpawnTimer()
    // {
    //     return spawnTimer;
    // }

    // public float GetSpawnTime()
    // {
    //     return spawnTime;
    // }

    // public override void ToggleMovement(bool toggle)
    // {
    //     base.ToggleMovement(toggle);

    //     if (toggle && initialBulletLaunch && (spawnIndex == 0))
    //     {
    //         availableMasks[spawnIndex].SwitchToActive();
    //         spawnIndex++;
    //     }
    // }
}
