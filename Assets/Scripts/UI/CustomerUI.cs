using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    public System.Action onOkAnimationComplete;

    private ActiveRequest _activeRequest;

    public Image customerPortrait;

    public Image requestOk;

    public ProductUI productUI;

    public Slider timerSlider;

    public Animator animator;

    private bool _isAngry = false;

    private RectTransform _rt; 

    public void Awake()
    {
        this.requestOk.gameObject.SetActive(false);

        this._rt = this.GetComponent<RectTransform>();
        LeanTween.moveY(this._rt, this._rt.rect.height, 0.8f)
            .setFrom(0f)
            .setEaseOutQuad();
    }

    public void UpdatePosition(float slotWidth)
    {
        if (this._activeRequest == null) return;
        this._rt.anchoredPosition = new Vector2(slotWidth * (this._activeRequest.slot + 0.5f), 0f);
    }

    public void SetActiveRequest(ActiveRequest activeRequest, float slotWidth)
    {
        this._activeRequest = activeRequest;
        this.productUI.SetRequest(activeRequest.request);
        this.UpdatePosition(slotWidth);
    }

    public void ShowOkAnimation()
    {
        this.requestOk.gameObject.SetActive(true);

        LeanTween.sequence()
            .append(
                LeanTween.scale(this.requestOk.gameObject, Vector3.one, 0.8f)
                    .setFrom(Vector3.zero)
                    .setEaseOutElastic()
                    .setOnComplete(this.onOkAnimationComplete)
            )
            .append(
                LeanTween.moveY(this._rt, 0f, 0.8f).setEaseInQuad()
            );
    }

    public void Update()
    {
        if (this._activeRequest == null) return;
        float p = this._activeRequest.progress;

        CustomerData customer = this._activeRequest.request.customer;
        this.customerPortrait.sprite = (p < 0.5f ? customer.customerAngryPortrait : customer.customerPortrait);

        this.customerPortrait.color = (p < 0.25f ? Color.red : Color.white);

        this.timerSlider.value = p;

        if (p < 0.25f && !this._isAngry)
        {
            this._isAngry = true;
            this.animator.Play("Angry");
        }
    }
}