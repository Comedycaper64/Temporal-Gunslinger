using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraMode
{
    fullbody,
    faceZoom,
    wideAngle,
    customCam1,
    customCam2,
    none,
}

[Serializable]
public struct Dialogue
{
    public ActorSO actor;
    public int actorNo;

    [TextArea]
    public string[] dialogue;
    public AnimationClip[] animations;
    public float[] animationTime;
    public CameraMode[] cameraModes;
    public bool disableCameraOnEnd;
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
