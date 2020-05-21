
using Hellmade.Sound;
using UnityEngine;

public class MainMenuUI : ScreenUI
{
    public WindowUI levelSelectionUI = null;
    public WindowUI optionsUI = null;
    public WindowUI creditsUI = null;

    private WindowUI _activeScreen = null;
    public AudioClip music = null;


    public void Start()
    {
        PlayerPrefsManager.LoadPrefs();
        AdsManager.instance.Init();
        
        this.levelSelectionUI.HideNow();
        this.optionsUI.HideNow();
        this.creditsUI.HideNow();

        if (this.music)
        {
            EazySoundManager.PlayMusic(this.music, 0.25f, true, false);
        }
        
        this.levelSelectionUI.onClosed += this._OnScreenClosed;
        this.optionsUI.onClosed += this._OnScreenClosed;
        this.creditsUI.onClosed += this._OnScreenClosed;
    }

    public void OnPlayButtonPressed()
    {
        this.SetActiveWindow(this.levelSelectionUI);
    }

    public void OnOptionsButtonPressed()
    {
        this.SetActiveWindow(this.optionsUI);
    }

    public void OnCreditsButtonPressed()
    {
        this.SetActiveWindow(this.creditsUI);
    }

    public void SetActiveWindow(WindowUI window)
    {
        this._activeScreen?.HideNow();
        window?.Show();
        this._activeScreen = window;
    }

    void _OnScreenClosed()
    {
        this._activeScreen = null;
    }
}