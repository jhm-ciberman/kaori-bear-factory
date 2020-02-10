using UnityEngine;

public class UnlockScreenUI : MonoBehaviour
{
    public void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowScreen()
    {
        this.gameObject.SetActive(true);
    }
}