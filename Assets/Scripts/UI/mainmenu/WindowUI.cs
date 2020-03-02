using UnityEngine;

public class WindowUI : MonoBehaviour
{
    public event System.Action onClosed;

    public void HideNow()
    {
        this.gameObject.SetActive(false);
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
}