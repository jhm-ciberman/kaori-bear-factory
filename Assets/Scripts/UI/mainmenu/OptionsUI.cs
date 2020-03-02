using UnityEngine;

public class OptionsUI : MonoBehaviour
{
    public WindowUI progressClearedPopupUI = null;

    public void Awake()
    {
        this.progressClearedPopupUI.HideNow();
    }

    public void OnClearProgressPressed()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        this.progressClearedPopupUI.Show();
    }
}