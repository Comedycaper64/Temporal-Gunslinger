using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : RewindableMovement
{
    private void Update()
    {
        transform.position += transform.forward * GetSpeed() * Time.deltaTime;
    }
}
