using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public CustomersLayoutUI customersLayoutUI;

    public LevelCompleteUI winScreen;

    public UnlockScreenUI unlockScreenUI;

    public RectTransform scrollView;

    void Start()
    {
        this.winScreen.onDone += () => this.unlockScreenUI.ShowFirstUnlockable();
        this.unlockScreenUI.onDone += () => SceneManager.LoadScene("Menu");

        this.winScreen.gameObject.SetActive(false);
        this.unlockScreenUI.gameObject.SetActive(false);
    }

    public void ShowLevelCompleteScreen(LevelData level)
    {
        this.customersLayoutUI.gameObject.SetActive(false);
        this.scrollView.gameObject.SetActive(false);

        foreach (var unlockable in level.unlockables)
        {
            this.unlockScreenUI.AddUnlockable(unlockable.name, unlockable.model);
        }

        this.winScreen.Show();
    }
}
