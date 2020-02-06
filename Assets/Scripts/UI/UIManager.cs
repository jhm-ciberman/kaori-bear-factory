using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public RectTransform cursor;

    public CustomersLayoutUI customersLayoutUI;

    public RectTransform winScreen;

    void Start()
    {
        this.winScreen.gameObject.SetActive(false);
    }
    
    public void SetCursorPosition(Vector2 pos)
    {
        this.cursor.anchoredPosition = pos * new Vector2(1, 1);
    }

    public void ShowWinScreen()
    {
        this.winScreen.gameObject.SetActive(true);
    }
}
