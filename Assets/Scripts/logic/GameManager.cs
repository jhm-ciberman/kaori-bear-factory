using Hellmade.Sound;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static LevelData currentLevelData;

    [SerializeField] private GameLevelsData _gameLevelsData = null;

    [SerializeField] private LevelData _testLevel = null;

    [SerializeField] private UIManager _uiManager = null;

    [SerializeField] private RequestsManager _requestsManager = null;

    [SerializeField] private PlayerInput _playerInput = null;

    [SerializeField] private PaintingMachine _paintingMachine = null;

    [SerializeField] private DeliveryBox _carboardBox = null;

    [SerializeField] private DeliveryBox _giftBox = null;

    [SerializeField] private Spawner _spawner = null;

    [SerializeField] private AudioClip _musicAudioClip = null;

    [HideInInspector] private LevelData _currentLevel;

    public void Start()
    {
        LevelManager.SetLevelsList(this._gameLevelsData);
        
        AdsManager.instance.Init();

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        //this._uiManager.gameObject.SetActive(true);
        this._SetupSceneEvents();

        // Start the fun!
        this._StartTheFun();

        if (this._musicAudioClip) 
        {
            EazySoundManager.PlayMusic(this._musicAudioClip, 0.25f, true, false);
        }
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

    private void _SetupSceneEvents()
    {
        // UI
        this._uiManager.onExitLevel += this._GoToMenu;
        this._uiManager.onNextLevel += this._NextLevel;
        this._uiManager.onRestartLevel += this._RestartLevel;
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
        this._requestsManager.onLevelFailed += this._OnLevelFailed;

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

    private void _NextLevel()
    {
        AdsManager.instance.ShowInterstitialAndThen(() => {
            var nextLevel = LevelManager.GetNextLevel(this._currentLevel);
            GameManager.currentLevelData = nextLevel;
            this._RestartLevel();
        });
    }

    private void _RestartLevel()
    {
        AdsManager.instance.ShowInterstitialAndThen(() => {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    private void _OnLevelComplete()
    {
        LevelManager.Win(this._currentLevel);

        this._playerInput.DisableInput();

        LeanTween.delayedCall(2f, () => {
            this._uiManager.ShowLevelCompleteScreen(this._currentLevel);
        });
    }

    private void _OnLevelFailed()
    {
        LevelManager.Fail(this._currentLevel);

        this._playerInput.DisableInput();

        LeanTween.delayedCall(2f, () => {
            this._uiManager.ShowLevelFailScreen();
        });
    }
}