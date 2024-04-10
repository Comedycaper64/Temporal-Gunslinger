using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : RewindableMovement
{
    [SerializeField]
    private Transform startTransform;

    private void Update()
    {
        //transform.position += transform.forward * GetSpeed() * Time.deltaTime;
        transform.position =
            startTransform.position + (transform.forward * GetSpeed() * GetRewindTime());
    }
}
