using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    public System.Action onExitAnimationComplete;

    private Request _request;

    public Image customerPortrait;

    public Image requestOk;
    
    public Image requestFailed;

    public ProductUI productUI;

    public Slider timerSlider;

    public Animator animator;

    private bool _isAngry = false;

    private RectTransform _rt; 

    public void Awake()
    {
        this.requestOk.gameObject.SetActive(false);
        this.requestFailed.gameObject.SetActive(false);

        this._rt = this.GetComponent<RectTransform>();
        LeanTween.moveY(this._rt, 0f, 0.8f)
            .setFrom(-this._rt.rect.height * this._rt.localScale.y)
            .setEaseOutQuad();
    }

    public void UpdatePosition(float slotWidth)
    {
        if (this._request == null) return;
        this._rt.anchoredPosition = new Vector2(slotWidth * (this._request.slot + 0.5f), 0f);
    }

    public void SetRequest(Request request, float slotWidth)
    {
        this._request = request;
        this.productUI.SetRequest(request);
        this.UpdatePosition(slotWidth);
    }

    public void ShowOkAnimation()
    {
        this.animator.Play("Normal");
        this._ShowAnimation(this.requestOk.gameObject, this.onExitAnimationComplete);
    }

    public void ShowFailAnimation()
    {
        this._ShowAnimation(this.requestFailed.gameObject, this.onExitAnimationComplete);
    }

    private void _ShowAnimation(GameObject go, System.Action callback)
    {
        go.SetActive(true);

        LeanTween.sequence()
            .append(
                LeanTween.scale(go, Vector3.one, 0.8f)
                    .setFrom(Vector3.zero)
                    .setEaseOutElastic()
            )
            .append(
                LeanTween.moveY(this._rt, -this._rt.rect.height * this._rt.localScale.y, 0.8f)
                    .setEaseInQuad()
                    .setOnComplete(callback)
            );
    } 

    public void Update()
    {
        if (this._request == null) return;
        float p = this._request.progress;

        CustomerData customer = this._request.customer;
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