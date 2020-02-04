using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public RectTransform cursor;

    public GameObject customerPrefab;

    public HorizontalLayoutGroup customersLayoutGroup;

    public RectTransform winScreen;

    private Dictionary<ActiveRequest, CustomerUI> _requestsUI = new Dictionary<ActiveRequest, CustomerUI>();

    // Start is called before the first frame update
    void Start()
    {
        this.winScreen.gameObject.SetActive(false);
    }

    public void SetCursorPosition(Vector2 pos)
    {
        this.cursor.anchoredPosition = pos * new Vector2(1, 1);
    }

    public void AddActiveRequest(ActiveRequest activeRequest)
    {
        GameObject go = Object.Instantiate(this.customerPrefab, Vector3.zero, Quaternion.identity, this.customersLayoutGroup.transform);
        CustomerUI ui = go.GetComponent<CustomerUI>();
        ui.SetActiveRequest(activeRequest);

        go.transform.SetAsFirstSibling();
        this._requestsUI.Add(activeRequest, ui);
    }

    public void RemoveActiveRequest(ActiveRequest activeRequest)
    {
        if (! this._requestsUI.ContainsKey(activeRequest)) return;
        CustomerUI ui = this._requestsUI[activeRequest];
        
        Destroy(ui.gameObject);
        this._requestsUI.Remove(activeRequest);
    }

    public void ShowWinScreen()
    {
        this.winScreen.gameObject.SetActive(true);
    }
}
