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

    public void Awake()
    {
        this.requestOk.gameObject.SetActive(false);

        RectTransform rt = this.GetComponent<RectTransform>();
        LeanTween.moveY(rt, rt.rect.height, 0.8f)
            .setFrom(0f)
            .setEaseOutQuad();
    }

    public void UpdatePosition(float slotWidth)
    {
        if (this._activeRequest == null) return;
        this.GetComponent<RectTransform>().anchoredPosition = new Vector2(slotWidth * (this._activeRequest.slot + 0.5f), 0f);
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
        LeanTween.scale(this.requestOk.gameObject, Vector3.one, 0.8f)
            .setFrom(Vector3.zero)
            .setEaseOutElastic()
            .setOnComplete(this.onOkAnimationComplete);
    }

    public void Update()
    {
        if (this._activeRequest == null) return;
        float p = 1f - (this._activeRequest.elapsedTime / this._activeRequest.request.maximumTime);

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