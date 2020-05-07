public class PauseScreenUI : ScreenUI
{
    public System.Action onUnpaused;
    public System.Action onGoToMainMenu;
    public System.Action onRestart;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void OnUnpuseButtonPressed()
    {
        this.HideNow();
        this.onUnpaused?.Invoke();
    }

    public void OnGoToMainMenuButtonPressed()
    {
        this.onGoToMainMenu?.Invoke();
    }

    public void OnRestartLevelPressed()
    {
        this.onRestart?.Invoke();
    }
}