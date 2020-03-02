using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectionUI : MonoBehaviour
{
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

    public void StartLevel(LevelData levelData)
    {
        GameManager.currentLevelData = levelData;
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
}