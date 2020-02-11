using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockScreenUI : MonoBehaviour
{
    public System.Action onDone;

    struct Unlockable
    {
        public string name;
        public GameObject model;
    }

    public Text title;
    
    public RectTransform overlay;

    public RectTransform flareBackground;

    public Transform unlockableObject;

    private Queue<Unlockable> _unlockables = new Queue<Unlockable>();

    public float modelRotationSpeed = 100f;
    public float backgroundRotationSpeed = 100f;

    public void AddUnlockable(string name, GameObject model)
    {
        this._unlockables.Enqueue(new Unlockable{name = name, model = model});
    }

    public void ShowFirstUnlockable()
    {
        if (this._unlockables.Count == 0)
        {
            this.onDone?.Invoke();
            return;
        }
        
        this.gameObject.SetActive(true);

        LeanTween.alpha(this.overlay, 0.75f, 1f);
        LeanTween.scale(this.flareBackground, Vector3.one, 0.5f);

        this._SetOverlayAlpha(0f);
        this.flareBackground.transform.localScale = Vector3.zero;

        this.OnContinueButtonPressed();
    }

    private void _SetOverlayAlpha(float alpha)
    {
        Image image = this.overlay.GetComponent<Image>();
        Color c = image.color;
        c.a = alpha;
        image.color = c;
    }

    public void OnContinueButtonPressed()
    {
        if (this._unlockables.Count == 0) 
        {
            this.onDone?.Invoke();
            return;
        }

        Unlockable unlockable = this._unlockables.Dequeue();

        this.title.text = unlockable.name + " unlocked!";
        foreach (Transform child in this.unlockableObject)
        {
            Object.Destroy(child.gameObject);
        }

        GameObject go = Object.Instantiate(unlockable.model, Vector3.zero, Quaternion.identity, this.unlockableObject);
        
        Vector3 endScale = go.transform.localScale;
        LeanTween.scale(go, endScale, 0.85f).setEaseOutBack();

        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.zero;
    }

    void Update()
    {
        this.unlockableObject.Rotate(Vector3.up * this.modelRotationSpeed * Time.deltaTime);
        this.flareBackground.Rotate(Vector3.forward * this.backgroundRotationSpeed * Time.deltaTime);
    }
}