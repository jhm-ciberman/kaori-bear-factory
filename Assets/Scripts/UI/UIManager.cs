using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public System.Action onExitLevel;
    public System.Action onPause;
    public System.Action onUnpause;

    [SerializeField] public CustomersLayoutUI _customersLayoutUI;

    [SerializeField] public LevelCompleteUI _levelCompleteUI;

    [SerializeField] public UnlockScreenUI _unlockScreenUI;

    [SerializeField] public PauseScreenUI _pauseScreenUI;

    [SerializeField] public OverlayUI _overlayUI;

    [SerializeField] public Transform _inGameUI;

    public TMPro.TextMeshProUGUI _customersCountText;

    void Start()
    {
        this._levelCompleteUI.onDone += () => {
            this._unlockScreenUI.overlay.alpha = this._overlayUI.alpha;
            this._overlayUI.HideNow();
            this._unlockScreenUI.ShowFirstUnlockable();
        };
        this._unlockScreenUI.onDone += () => this.onExitLevel?.Invoke();
        this._pauseScreenUI.onUnpaused += () => {
            this.onUnpause?.Invoke();
            this._overlayUI.HideNow();
        };

        this._customersLayoutUI.gameObject.SetActive(true);
        this._levelCompleteUI.HideNow();
        this._unlockScreenUI.HideNow();
        this._pauseScreenUI.HideNow();
        this._overlayUI.HideNow();
        this._inGameUI.gameObject.SetActive(true);
    }

    public void SetSlotsNumber(int slots)
    {
        this._customersLayoutUI.slotsNumber = slots;
    }

    public void SetCustomersCountText(string text)
    {
        this._customersCountText.text = text;
    }

    public void ShowLevelCompleteScreen(LevelData level)
    {
        this._inGameUI.gameObject.SetActive(false);
        this._customersLayoutUI.gameObject.SetActive(false);

        this._overlayUI.Show();

        foreach (var unlockable in level.unlockables)
        {
            this._unlockScreenUI.AddUnlockable(unlockable.name, unlockable.model);
        }

        this._levelCompleteUI.Show();
    }

    public void AddRequest(Request request)
    {
        this._customersLayoutUI.AddRequest(request);
    }

    public void RemoveRequest(Request request)
    {
        this._customersLayoutUI.RemoveRequest(request);
    }

    public void OnMenuButtonPressed()
    {
        this.onPause?.Invoke();
        this._overlayUI.Show();
        this._pauseScreenUI.Show();
    }
}
