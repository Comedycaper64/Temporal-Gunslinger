using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeUI", menuName = "Cinematic Node/UIChangeSO", order = 0)]
public class UIChangeSO : CinematicNode
{
    public bool fadeToBlackToggle;
    public bool waitUntilFaded;
    public Action onFaded;
}
