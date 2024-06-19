using UnityEngine;
using UnityEngine.SceneManagement;

public class CinematicPlayer : GameManager
{
    [SerializeField]
    private LevelSelectorUI levelSelectUI;

    public override void Start()
    {
        OnGameStateChange?.Invoke(this, StateEnum.inactive);

        CinematicManager.Instance.PlayCinematic(levelIntroCinematic, OpenPocketwatchUI);
    }

    protected override void OnDisable() { }

    private void OpenPocketwatchUI()
    {
        levelSelectUI.ToggleLevelSelector(true);
    }

    protected override void LoadNextLevel()
    {
        SceneManager.LoadScene(0);
    }

    public void SelectNextLevel()
    {
        CinematicManager.Instance.PlayCinematic(levelOutroCinematic, LoadNextLevel);
    }
}
