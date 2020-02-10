using UnityEngine;

public class UnlockScreenUI : MonoBehaviour
{
    public Transform unlockableObject;

    public float rotationSpeed = 100f;

    public void Start()
    {
        this.gameObject.SetActive(false);
        this.ShowScreen();
    }

    public void ShowScreen()
    {
        this.gameObject.SetActive(true);
    }

    void Update()
    {
        this.unlockableObject.Rotate(Vector3.up * this.rotationSpeed * Time.deltaTime);
    }
}