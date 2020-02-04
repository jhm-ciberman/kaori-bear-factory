using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour
{
    private ActiveRequest _activeRequest;

    public Image customerPortrait;

    public ProductUI productUI;

    public Slider timerSlider;

    public Animator animator;

    private bool _isAngry = false;

    public void SetActiveRequest(ActiveRequest activeRequest)
    {
        this._activeRequest = activeRequest;
        this.productUI.SetRequest(activeRequest.request);
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