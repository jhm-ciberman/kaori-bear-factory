using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenUI : ScreenUI
{
    public System.Action onUnpaused;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void OnUnapuseButtonPressed()
    {
        this.HideNow();
        this.onUnpaused?.Invoke();
    }

    public void OnGoToMainMenuButtonPressed()
    {
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}