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
    }
}
