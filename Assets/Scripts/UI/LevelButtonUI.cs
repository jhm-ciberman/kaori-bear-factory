using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    public System.Action<LevelData> onLevelSelect;
    
    public GameObject levelCompleteImage;

    public TMPro.TextMeshProUGUI levelTitleText;

    public TMPro.TextMeshProUGUI levelDayText;

    private LevelData _level;

    public void SetLevel(LevelData level, string dayText)
    {
        this._level = level;

        bool levelIsComplete = (PlayerPrefs.GetInt("Level_" + level.name, 0) == 1);

        this.levelCompleteImage.SetActive(levelIsComplete);

        this.levelDayText.text = dayText;
        this.levelTitleText.text = level.displayName;
    }

    public void OnClick()
    {
        this.onLevelSelect?.Invoke(this._level);
    }

    
}