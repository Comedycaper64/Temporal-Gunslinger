using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeUI", menuName = "Cinematic Node/UIChangeSO", order = 0)]
public class UIChangeSO : CinematicNode
{
    public bool fadeToBlackToggle;
    public bool waitUntilFaded;
    public bool midLevelFade = false;
    public Action onFaded;
}
