
using UnityEngine;

public class MainMenuUI : ScreenUI
{
    public LevelSelectionUI levelSelectionUI = null;
    public CreditsUI creditsUI = null;

    private ScreenUI _activeScreen = null;

    public void Start()
    {
        this.levelSelectionUI.HideNow();
        this.creditsUI.HideNow();
        
        this.levelSelectionUI.onClosed += this._OnScreenClosed;
        this.creditsUI.onClosed += this._OnScreenClosed;
    }

    public void OnPlayButtonPressed()
    {
        this._activeScreen?.HideNow();
        this.levelSelectionUI.Show();
        this._activeScreen = this.levelSelectionUI;
    }

    public void OnOptionsButtonPressed()
    {
        this._activeScreen?.HideNow();
        this.levelSelectionUI.Show();
        this._activeScreen = this.levelSelectionUI;
    }

    public void OnCreditsButtonPressed()
    {
        this._activeScreen?.HideNow();
        this.creditsUI.Show();
        this._activeScreen = this.creditsUI;
    }

    void _OnScreenClosed()
    {
        this._activeScreen = null;
    }

    public void ClearProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }
}