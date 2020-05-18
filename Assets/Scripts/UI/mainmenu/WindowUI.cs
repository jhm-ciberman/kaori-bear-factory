using UnityEngine;

public class WindowUI : MonoBehaviour
{
    public event System.Action onClosed;

    public RectTransform element = null;

    private Vector3 _initialScale; 

    public void HideNow()
    {
        this.gameObject.SetActive(false);
    }

    private void Awake()
    {
        this.element = (this.element != null) ? this.element : this.GetComponent<RectTransform>();
        this._initialScale = this.element.localScale;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        this.element.localScale = Vector3.zero;
        LeanTween.scale(this.element, this._initialScale, .5f)
            .setEaseOutBack();
    }

    public void OnCloseButtonPressed()
    {
        this.HideNow();
        this.onClosed?.Invoke();
    }
}