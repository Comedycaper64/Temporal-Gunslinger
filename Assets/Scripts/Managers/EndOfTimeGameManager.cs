using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfTimeGameManager : GameManager
{
    [SerializeField]
    private LevelSelectorUI levelSelectUI;

    public override void Start()
    {
        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, ShowLevelSelect);
    }

    protected override void OnDisable() { }

    private void ShowLevelSelect()
    {
        levelSelectUI.ToggleLevelSelector(true);
    }
}
