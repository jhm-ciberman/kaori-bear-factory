using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionUI : ScreenUI
{
    public event System.Action onClosed;

    public GameLevelsData levelsData;

    public LevelButtonUI baseButton;

    public void Start()
    {
        Transform parent = this.baseButton.transform.parent;

        int levelNumber = 1;
        foreach (LevelData level in this.levelsData.levels)
        {
            LevelButtonUI button = Object.Instantiate(this.baseButton, Vector3.zero, Quaternion.identity, parent);
            button.onLevelSelect += this.StartLevel;
            button.SetLevel(level, "Day " + levelNumber);
            levelNumber++;
        }

        Object.Destroy(this.baseButton.gameObject);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void OnCloseButtonPressed()
    {
        this.HideNow();
        this.onClosed?.Invoke();
    }

    public void StartLevel(LevelData levelData)
    {
        GameManager.currentLevelData = levelData;
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
}