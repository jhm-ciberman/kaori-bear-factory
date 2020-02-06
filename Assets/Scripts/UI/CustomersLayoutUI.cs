using System.Collections.Generic;
using UnityEngine;

public class CustomersLayoutUI : MonoBehaviour
{
    public RequestsManager requestsManager;

    public GameObject customerPrefab;

    private Dictionary<ActiveRequest, CustomerUI> _requestsUI = new Dictionary<ActiveRequest, CustomerUI>();

    private RectTransform _rt;

#if UNITY_EDITOR
     private Vector2 _resolution;
#endif //UNITY_EDITOR

    void Start()
    {
        this._rt = this.GetComponent<RectTransform>();

        this.requestsManager.onActiveRequestAdded += this.AddActiveRequest;
        this.requestsManager.onActiveRequestRemoved += this.RemoveActiveRequest;

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

    public void AddActiveRequest(ActiveRequest activeRequest)
    {
        GameObject go = Object.Instantiate(this.customerPrefab, Vector3.zero, Quaternion.identity, this.transform);

        CustomerUI ui = go.GetComponent<CustomerUI>();
        ui.SetActiveRequest(activeRequest, this._GetSlotWidth());

        go.transform.SetAsFirstSibling();
        this._requestsUI.Add(activeRequest, ui);
    }

    public void RemoveActiveRequest(ActiveRequest activeRequest)
    {
        if (! this._requestsUI.ContainsKey(activeRequest)) return;
        CustomerUI ui = this._requestsUI[activeRequest];
        ui.onOkAnimationComplete = () => {
            Destroy(ui.gameObject, 1f);
            this._requestsUI.Remove(activeRequest);
        };
        ui.ShowOkAnimation();
    }

}