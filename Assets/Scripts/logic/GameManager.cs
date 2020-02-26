using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static LevelData currentLevelData;

    [SerializeField] private LevelData _testLevel = null;

    [SerializeField] public UIManager _uiManager = null;

    [SerializeField] public RequestsManager _requestsManager = null;

    [SerializeField] public PlayerInput _playerInput = null;

    [HideInInspector]
    private LevelData _currentLevel;

    public void Start()
    {
        this._uiManager.gameObject.SetActive(true);
        
        // Level exit UI
        this._uiManager.onExitLevel += this._GoToMenu;

        // Requests handling
        this._requestsManager.onActiveRequestAdded += this._uiManager.AddRequest;
        this._requestsManager.onActiveRequestCompleted += this._uiManager.RemoveRequest;
        this._requestsManager.onActiveRequestFailed += this._uiManager.RemoveRequest;

        // UI update
        this._requestsManager.onActiveRequestCompleted += (_req) => this._UpdateCustomersUI();
        this._requestsManager.onActiveRequestFailed += (_req) => this._UpdateCustomersUI();

        // Level complete
        this._requestsManager.onLevelComplete += this._OnLevelComplete;

        // Pause
        this._uiManager.onPause += this._OnPause;
        this._uiManager.onUnpause += this._OnUnpause;

        // Start the fun!
        this._currentLevel = (GameManager.currentLevelData == null) ? this._testLevel : GameManager.currentLevelData;
        this._uiManager.SetSlotsNumber(this._currentLevel.slotsNumber);
        this._requestsManager.StartLevel(this._currentLevel);

        this._UpdateCustomersUI();
    }

    private void _UpdateCustomersUI()
    {
        int total = this._currentLevel.requests.Length;
        int inQueue = this._requestsManager.requestsInQueueCount;
        int active = this._requestsManager.requestsActiveCount;
        int notFullfiled = total - inQueue - active;
        string str = notFullfiled + " / " + total;
        this._uiManager.SetCustomersCountText(str);
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
        this._playerInput.DisableInput();
    }

    private void _OnUnpause()
    {
        Time.timeScale = 1f;
        this._playerInput.EnableInput();
    }

    private void _GoToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private void _OnLevelComplete(LevelData level)
    {
        PlayerPrefs.SetInt("Level_" + level.name, 1);
        PlayerPrefs.Save();

        this._playerInput.DisableInput();

        LeanTween.delayedCall(2f, () => {
            this._uiManager.ShowLevelCompleteScreen(level);
        });
    }
}