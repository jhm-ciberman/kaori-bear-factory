using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static LevelData currentLevelData;

    [SerializeField]
    private LevelData _testLevel;

    [SerializeField]
    public UIManager _uiManager;

    [SerializeField]
    public RequestsManager _requestsManager;

    [SerializeField]
    public PlayerInteraction _playerInteraction;

    [SerializeField]
    public PlayerMovement _playerMovement;

    public void Start()
    {
        // Level exit UI
        this._uiManager.onExitLevel += this._GoToMenu;

        // Requests handling
        this._requestsManager.onActiveRequestAdded += this._uiManager.AddRequest;
        this._requestsManager.onActiveRequestCompleted += this._uiManager.RemoveRequest;
        this._requestsManager.onActiveRequestFailed += this._uiManager.RemoveRequest;

        // Level complete
        this._requestsManager.onLevelComplete += this._OnLevelComplete;

        // Pause
        this._uiManager.onPause += this._OnPause;
        this._uiManager.onUnpause += this._OnUnpause;

        // Start the fun!
        LevelData level = (GameManager.currentLevelData == null) ? this._testLevel : GameManager.currentLevelData;
        this._uiManager.SetSlotsNumber(level.slotsNumber);
        this._requestsManager.StartLevel(level);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this._requestsManager.CompleteLevel();
        }
#endif
    }

    private void _OnPause()
    {
        Time.timeScale = 0f;
        this._playerInteraction.DisableInteraction();
        this._playerMovement.DisableMovement();
    }

    private void _OnUnpause()
    {
        Time.timeScale = 1f;
        this._playerInteraction.EnableInteraction();
        this._playerMovement.EnableMovement();
    }

    private void _GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void _OnLevelComplete(LevelData level)
    {
        PlayerPrefs.SetInt("Level_" + level.name, 1);
        PlayerPrefs.Save();

        this._playerInteraction.DisableInteraction();
        this._playerMovement.DisableMovement();

        LeanTween.delayedCall(2f, () => {
            this._uiManager.ShowLevelCompleteScreen(level);
        });
    }
}