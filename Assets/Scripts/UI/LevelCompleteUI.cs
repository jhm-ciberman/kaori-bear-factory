using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class LevelCompleteUI : MonoBehaviour
{
    public System.Action onDone;

    public RectTransform screen;

    public RectTransform lights;

    private float _GetHeight()
    {
        return this.GetComponent<RectTransform>().rect.height;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        float y = this.screen.anchoredPosition.y;
        this.screen.anchoredPosition = new Vector2(this.screen.anchoredPosition.x, -this._GetHeight());

        LeanTween.moveY(this.screen, y, 0.8f).setEaseOutBack();

        LeanTween.alpha(this.lights, 0f, 0.8f).setLoopPingPong();
    }

    public void OnContinueButtonPressed()
    {
        LeanTween.moveY(this.screen, -this._GetHeight(), 0.8f)
            .setEaseOutBack()
            .setOnComplete(() => this.onDone?.Invoke());
    }
}