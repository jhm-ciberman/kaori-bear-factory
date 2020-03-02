using UnityEngine;
using UnityEngine.UI;

public class OverlayUI : ScreenUI
{
    public Image image;

    public void Show()
    {
        this.image.gameObject.SetActive(true);
        LeanTween.alpha(this.image.GetComponent<RectTransform>(), 0.8f, 0.5f)
            .setEaseOutQuad()
            .setIgnoreTimeScale(true);
    }

    public new void HideNow()
    {
        this.alpha = 0f;
        this.image.gameObject.SetActive(false);
    }

    public float alpha
    {
        get => this.image.color.a;
        set {
            Color c = this.image.color;
            this.image.color = new Color(c.r, c.g, c.b, value);
        }
    }

}