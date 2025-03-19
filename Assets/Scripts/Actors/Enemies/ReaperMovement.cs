using UnityEngine;

public class ReaperMovement : RewindableMovement
{
    private void Update()
    {
        transform.position += -transform.forward * GetSpeed() * Time.deltaTime;
    }
}
