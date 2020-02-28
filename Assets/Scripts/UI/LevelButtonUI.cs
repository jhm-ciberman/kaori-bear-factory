using UnityEngine;
using UnityEngine.UI;

public class LevelButtonUI : MonoBehaviour
{
    public System.Action<LevelData> onLevelSelect;
    
    public GameObject levelCompleteImage;

    public TMPro.TextMeshProUGUI displayText;

    private LevelData _level;

    public void SetLevel(LevelData level)
    {
        this._level = level;

        bool levelIsComplete = (PlayerPrefs.GetInt("Level_" + level.name, 0) == 1);

        this.levelCompleteImage.SetActive(levelIsComplete);

        this.displayText.text = level.displayName;
    }

    public void OnClick()
    {
        this.onLevelSelect?.Invoke(this._level);
    }
}