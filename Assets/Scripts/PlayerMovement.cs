using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private RectTransform _screen;

    [SerializeField]
    private ScrollRect _scrollView;

    [SerializeField]
    private float _moveSpeed = 1f;

    [SerializeField]
    private float _scrollLengthRatio = 0.2f;

    [SerializeField]
    private Transform[] _stations;

    [SerializeField]
    private float _initialStation = 0;

    private bool _canMove = true;

    void Start()
    {
        foreach (Transform station in this._stations)
        {
            station.gameObject.SetActive(false);
        }

        this._scrollView.content.sizeDelta = new Vector2(this._screen.sizeDelta.x * this._stations.Length * this._scrollLengthRatio, this._screen.sizeDelta.y);
        this.SetStation(this._initialStation);
        this._viewPos = this._initialStation / (this._stations.Length - 1);
    }

    public void EnableMovement()
    {
        this._canMove = true;
        this._scrollView?.gameObject?.SetActive(true);
    }

    public void DisableMovement()
    {
        this._canMove = false;
        this._scrollView?.gameObject?.SetActive(false);
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

    public void OnScrollViewPosChange(Vector2 value)
    {
        if (! this._canMove) return;
        this.SetNormalizedValue(value.x);
    }

    public void SetNormalizedValue(float value)
    {
        this.SetStation(value * (this._stations.Length - 1));
    }

    public void SetStation(float value)
    {
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