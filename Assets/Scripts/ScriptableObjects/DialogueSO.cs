using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    fullbody,
    faceZoom,
    wideAngle,
    none,
}

[Serializable]
public struct Dialogue
{
    public ActorSO actor;
    public int actorNo;
    public string[] dialogue;
    public AnimationClip[] animations;
    public float[] animationTime;
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
