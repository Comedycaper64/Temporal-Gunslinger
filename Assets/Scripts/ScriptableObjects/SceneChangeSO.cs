using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SceneChange", menuName = "Cinematic Node/SceneChangeSO", order = 0)]
public class SceneChangeSO : CinematicNode
{
    public bool startScrollingWalk;
    public bool fadeOutMusic;
    public bool fadeInMusic;
    public AudioClip musicTrackChange;
}
