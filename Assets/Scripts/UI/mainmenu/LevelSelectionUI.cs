using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    public GameLevelsData levelsData;

    public LevelButtonUI baseButton;

    public ScrollRect scrollRect = null;

    public void Start()
    {
        Transform parent = this.baseButton.transform.parent;

        int levelNumber = 1;
        foreach (LevelData level in this.levelsData.levels)
        {
            LevelButtonUI button = Object.Instantiate(this.baseButton, Vector3.zero, Quaternion.identity, parent);
            button.onLevelSelect += this.StartLevel;
            button.SetLevel(level, "Day " + levelNumber);
            levelNumber++;
        }

        Object.Destroy(this.baseButton.gameObject);
        
        this.StartCoroutine(this._FixUndesiredMovement(parent));
    }

    private IEnumerator _FixUndesiredMovement(Transform parent)
    {
        yield return new WaitForEndOfFrame();
        this.scrollRect.verticalNormalizedPosition = 1f;
        this.scrollRect.StopMovement();
        parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    public void StartLevel(LevelData levelData)
    {
        GameManager.currentLevelData = levelData;
        SceneManager.LoadScene("MainGame", LoadSceneMode.Single);
    }
}