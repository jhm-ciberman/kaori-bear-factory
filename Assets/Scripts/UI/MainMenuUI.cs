using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : ScreenUI
{
    [ReorderableList]
    public LevelData[] levels;

    public LevelButtonUI baseButton;

    public void Start()
    {
        Transform parent = this.baseButton.transform.parent;

        foreach (LevelData level in this.levels)
        {
            LevelButtonUI button = Object.Instantiate(this.baseButton, Vector3.zero, Quaternion.identity, parent);
            button.onLevelSelect += this.StartLevel;
            button.SetLevel(level);
        }

        Object.Destroy(this.baseButton.gameObject);
    }

    public void ClearProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void StartLevel(LevelData levelData)
    {
        GameManager.currentLevelData = levelData;
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
}