using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    fullbody,
    faceZoom,
    wideAngle
}

[Serializable]
public struct Dialogue
{
    public ActorSO actor;
    public string[] dialogue;
    public AnimationClip[] animations;
    public CameraMode[] cameraModes;
}

[Serializable]
[CreateAssetMenu(fileName = "Dialogue", menuName = "Temporal Gunslinger/DialogueSO", order = 0)]
public class DialogueSO : CinematicNode
{
    [SerializeField]
    private Dialogue[] dialogues;

    public Dialogue[] GetDialogues()
    {
        return dialogues;
    }
}
