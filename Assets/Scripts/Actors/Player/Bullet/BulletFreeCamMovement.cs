using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFreeCamMovement : MonoBehaviour
{
    private bool bCanMove = false;

    public void ToggleCamMovement(bool toggle)
    {
        bCanMove = toggle;
    }
}
