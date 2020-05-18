using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    public System.Action<LevelData> onLevelSelect;
    
    public GameObject levelCompleteImage;

    public TMPro.TextMeshProUGUI levelTitleText;

    public TMPro.TextMeshProUGUI levelDayText;

    private LevelData _level;

    private string _dayText;

    public void SetLevel(LevelData level, string dayText)
    {
        this._level = level;
        this._dayText = dayText;

        this._UpdateText();

        LevelManager.onProgressReset += this._UpdateText;
    }

    private void _UpdateText()
    {
        bool levelIsComplete = LevelManager.GetLevelIsComplete(this._level);

        this.levelCompleteImage.SetActive(levelIsComplete);

        this.levelDayText.text = this._dayText;
        this.levelTitleText.text = this._level.displayName;
    }

    public void OnClick()
    {
        this.onLevelSelect?.Invoke(this._level);
    }

    
}