using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CustomersLayoutUI : MonoBehaviour
{
    public GameObject customerPrefab;

    private Dictionary<Request, CustomerUI> _requestsUI = new Dictionary<Request, CustomerUI>();

    private RectTransform _rt;

    [ReadOnly]
    public int slotsNumber = 1;

    private Vector2 _resolution;

    void Start()
    {
        this._rt = this.GetComponent<RectTransform>();
    }

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

    private float _GetSlotWidth()
    {
        return this._rt.rect.width / this.slotsNumber;
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