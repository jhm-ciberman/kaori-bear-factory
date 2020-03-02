public class CreditsUI : ScreenUI
{
    public event System.Action onClosed;

    public void Show()
    {
        this.gameObject.SetActive(true);
    }

    public void OnCloseButtonPressed()
    {
        this.HideNow();
        this.onClosed?.Invoke();
    }
}