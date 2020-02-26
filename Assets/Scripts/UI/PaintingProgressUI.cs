using UnityEngine;
using UnityEngine.UI;

public class PaintingProgressUI : MonoBehaviour 
{
    [SerializeField] private Slider _slider = null;

    [SerializeField] private Image _fillImage = null;

    public void Start()
    {

    }

    public float progress
    {
        get => this._slider.normalizedValue;
        set => this._slider.normalizedValue = value;
    }

    public void Show(Color color)
    {
        this.gameObject.SetActive(true);
        this._fillImage.color = color;
    }

    public void HideNow()
    {
        this.gameObject.SetActive(false);
    }

    public void Finish()
    {
        LeanTween.scale(this._slider.GetComponent<RectTransform>(), 1.1f * Vector3.one, 0.8f)
            .setEasePunch();
    }

}