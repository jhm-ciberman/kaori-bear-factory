using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    public void StartGame(LevelData levelData)
    {
        RequestsManager.currentLevelData = levelData;
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
}