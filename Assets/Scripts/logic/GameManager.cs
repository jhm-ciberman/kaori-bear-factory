using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static LevelData currentLevelData;

    [SerializeField] private LevelData _testLevel = null;

    [SerializeField] public UIManager _uiManager = null;

    [SerializeField] public RequestsManager _requestsManager = null;

    [SerializeField] public PlayerInput _playerInput = null;

    [SerializeField] public PaintingMachine _paintingMachine = null;

    [SerializeField] public DeliveryBox _carboardBox = null;

    [SerializeField] public DeliveryBox _giftBox = null;

    [SerializeField] public Spawner _spawner = null;

    [HideInInspector]
    private LevelData _currentLevel;

    public void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        //this._uiManager.gameObject.SetActive(true);
        this._SetupSceneEvents();

        // Start the fun!
        this._StartTheFun();
    }

    private void _StartTheFun() 
    {
        LevelData level = (GameManager.currentLevelData == null) 
            ? this._testLevel 
            : GameManager.currentLevelData;

        if (level.tutorialUI != null) 
        {
            this._playerInput.DisableInput();
            this._uiManager.OpenTutorialUI(level.tutorialUI, () => this.StartLevel(level));
        }
        else
        {
            this.StartLevel(level);
        }
    }

    private void _OpenTutorialUI(LevelData level)
    {

    }

    private void _SetupSceneEvents()
    {
        // UI
        this._uiManager.onExitLevel += this._GoToMenu;
        this._uiManager.onPause += this._OnPause;
        this._uiManager.onUnpause += this._OnUnpause;

        // Requests handling
        this._requestsManager.onActiveRequestAdded += this._uiManager.AddRequest;
        this._requestsManager.onActiveRequestCompleted += this._uiManager.RemoveRequest;
        this._requestsManager.onActiveRequestFailed += this._uiManager.RemoveRequest;

        // UI update
        this._requestsManager.onActiveRequestCompleted += (_req) => this._UpdateCustomersUI();
        this._requestsManager.onActiveRequestFailed += (_req) => this._UpdateCustomersUI();

        // Level complete
        this._requestsManager.onLevelComplete += this._OnLevelComplete;

        // Painting Machine
        this._paintingMachine.onPaintStart += (skin) => this._uiManager.ShowPaintingProgress(skin);
        this._paintingMachine.onPaintFinish += (skin) => this._uiManager.PaintingProgressFinish();
        this._paintingMachine.onPieceRemoved += () => this._uiManager.HidePaintingProgress();
        this._paintingMachine.onPaintProgress += (amount, total) => this._uiManager.UpdatePaintingProgress(amount, total);

        // DeliveryBox
        this._carboardBox.onCraftableDelivered += this._requestsManager.DeliverCraftable;
        this._giftBox.onCraftableDelivered += this._requestsManager.DeliverCraftable;
    }

    public void StartLevel(LevelData level)
    {
        this._currentLevel = level;
        this._uiManager.SetSlotsNumber(this._currentLevel.slotsNumber);
        this._giftBox.gameObject.SetActive(this._currentLevel.giftBoxUnlocked);
        this._paintingMachine.SetAvailableSkins(this._currentLevel.availableSkins);
        this._paintingMachine.timePerPiece = this._currentLevel.paintingTimePerPiece;
        this._spawner.defaultSkin = this._currentLevel.availableSkins[0];
        this._requestsManager.StartLevel(this._currentLevel);
        this._playerInput.EnableInput();
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

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this._requestsManager.CompleteLevel();
        }
    }
#endif

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
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    private void _OnLevelComplete()
    {
        PlayerPrefs.SetInt("Level_" + this._currentLevel.name, 1);
        PlayerPrefs.Save();

        this._playerInput.DisableInput();

        LeanTween.delayedCall(2f, () => {
            this._uiManager.ShowLevelCompleteScreen(this._currentLevel);
        });
    }
}