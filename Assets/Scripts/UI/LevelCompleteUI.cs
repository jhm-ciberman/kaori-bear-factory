using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteUI : MonoBehaviour
{
    public RectTransform overlay;

    public RectTransform screen;

    public RectTransform lights;

    public void Show()
    {
        LeanTween.alpha(this.overlay, 0.75f, 1f)
            .setFrom(0f);

        float y = this.screen.anchoredPosition.y;
        this.screen.anchoredPosition = new Vector2(this.screen.anchoredPosition.x, -Screen.height * 0.85f);

        LeanTween.moveY(this.screen, y, 0.8f)
            .setDelay(1f)
            .setEaseOutBack();

        LeanTween.alpha(this.lights, 0f, 0.8f)
            .setLoopPingPong();
    }

    public void OnContinueButtonPressed()
    {
        PlayerPrefs.SetInt("Level_" + RequestsManager.currentLevelData.name, 1);
        PlayerPrefs.Save();
        
        SceneManager.LoadScene("Menu");
    }
}