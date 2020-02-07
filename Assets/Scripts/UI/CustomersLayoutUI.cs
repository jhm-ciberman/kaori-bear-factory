using System.Collections.Generic;
using UnityEngine;

public class CustomersLayoutUI : MonoBehaviour
{
    public RequestsManager requestsManager;

    public GameObject customerPrefab;

    private Dictionary<Request, CustomerUI> _requestsUI = new Dictionary<Request, CustomerUI>();

    private RectTransform _rt;

#if UNITY_EDITOR
     private Vector2 _resolution;
#endif //UNITY_EDITOR

    void Start()
    {
        this._rt = this.GetComponent<RectTransform>();

        this.requestsManager.onActiveRequestAdded += this.AddRequest;
        this.requestsManager.onActiveRequestCompleted += this.RemoveRequest;
        this.requestsManager.onActiveRequestFailed += this.RemoveRequest;

#if UNITY_EDITOR
        this._resolution = new Vector2(Screen.width, Screen.height);
#endif //UNITY_EDITOR
    }

#if UNITY_EDITOR
     private void Update ()
     {
         if (this._resolution.x != Screen.width || this._resolution.y != Screen.height)
         {
             float slotWidth = this._GetSlotWidth();
             foreach (CustomerUI ui in this._requestsUI.Values)
             {
                 ui.UpdatePosition(slotWidth);
             }
            
             this._resolution.x = Screen.width;
             this._resolution.y = Screen.height;
         }
     }
#endif //UNITY_EDITOR

    private float _GetSlotWidth()
    {
        return this._rt.rect.width / this.requestsManager.level.slotsNumber;
    }

    public void AddRequest(Request request)
    {
        GameObject go = Object.Instantiate(this.customerPrefab, Vector3.zero, Quaternion.identity, this.transform);

        CustomerUI ui = go.GetComponent<CustomerUI>();
        ui.SetRequest(request, this._GetSlotWidth());

        go.transform.SetAsFirstSibling();
        this._requestsUI.Add(request, ui);
    }

    public void RemoveRequest(Request request)
    {
        if (! this._requestsUI.ContainsKey(request)) return;
        CustomerUI ui = this._requestsUI[request];
        ui.onExitAnimationComplete = () => {
            Object.Destroy(ui.gameObject, 1f);
            this._requestsUI.Remove(request);
        };

        if (request.failed)
        {
            ui.ShowFailAnimation();
        }
        else
        {
            ui.ShowOkAnimation();
        }
    }

}