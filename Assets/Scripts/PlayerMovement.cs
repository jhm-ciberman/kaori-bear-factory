using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private RequestsManager _requestsManager;

    [SerializeField]
    private ScrollRect _scrollView;

    [SerializeField]
    private float _moveSpeed = 1f;

    public Transform[] _stations;

    void Start()
    {
        this._scrollView.content.sizeDelta = new Vector2(Screen.width * this._stations.Length, Screen.height);
        this.SetNormalizedValue(this._scrollView.horizontalNormalizedPosition);

        foreach (Transform station in this._stations)
        {
            station.gameObject.SetActive(false);
        }

        this._requestsManager.onLevelComplete += () => this.enabled = false;
    }

    public void OnEnable()
    {
        
        this._scrollView?.gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        this._scrollView?.gameObject.SetActive(false);
    }


    public void MoveView(float delta)
    {
        this._viewPos += delta * this._moveSpeed;

        if (delta > 0f)
        {
            if (this._viewPos > 1f) this._viewPos = 1f;
        }
        else if (delta < 0f)
        {
            if (this._viewPos < 0f) this._viewPos = 0f;
        }

        this.SetNormalizedValue(this._viewPos);
    }

    private float _viewPos
    {
        get => this._scrollView.horizontalNormalizedPosition;
        set => this._scrollView.horizontalNormalizedPosition = value;
    }

    public void SetNormalizedValue(Vector2 value)
    {
        this.SetNormalizedValue(value.x);
    }

    public void SetNormalizedValue(float value)
    {
        value = value * (this._stations.Length - 1);

        if (value < 1f)
        {
            this._SetCamera(0, 1, value);
            return;
        }

        if (value > this._stations.Length - 2)
        {
            this._SetCamera(this._stations.Length - 2, this._stations.Length - 1, value - (this._stations.Length - 2));
            return;
        }

        int a = Mathf.FloorToInt(value);
        int b = Mathf.CeilToInt(value);
        this._SetCamera(a, b, value - a);
    }

    protected void _SetCamera(int a, int b, float t)
    {
        Transform stationA = this._stations[a];
        Transform stationB = this._stations[b];
        this.transform.position = Vector3.LerpUnclamped   (stationA.position, stationB.position, t);
        this.transform.rotation = Quaternion.LerpUnclamped(stationA.rotation, stationB.rotation, t);
    }
}