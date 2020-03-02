using UnityEngine;

public class WindowUI : MonoBehaviour
{
    public event System.Action onClosed;

    public RectTransform element = null;

    public void HideNow()
    {
        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        var rt = (this.element != null) ? this.element : this.GetComponent<RectTransform>();
        var scale = rt.localScale;
        rt.localScale = Vector3.zero;
        LeanTween.scale(rt, scale, .5f)
            .setEaseOutBack();
    }

    public void OnCloseButtonPressed()
    {
        this.HideNow();
        this.onClosed?.Invoke();
    }
}