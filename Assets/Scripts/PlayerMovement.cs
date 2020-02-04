using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private int _currentStation = 0;

    public Transform[] stations;

    public float animationDuration = 0.5f;

    private float _animationProgress = 0f;

    private Vector3 _positionStart;
    private Quaternion _rotationStart;

    public void Start()
    {
        
    }

    public void Move(int dir)
    {
        this._positionStart = this.transform.position;
        this._rotationStart = this.transform.rotation;

        int station = this._currentStation + dir;

        if (station < 0) station = 0;
        if (station > this.stations.Length - 1) station = this.stations.Length - 1;

        this._currentStation = station;
        this._animationProgress = 0f;
    }

    void Update()
    {
        if (this._animationProgress < this.animationDuration)
        {
            this._animationProgress += Time.deltaTime;

            float p = this._animationProgress / this.animationDuration;

            Transform station = this.stations[this._currentStation];
            Vector3 positionEnd = station.position;
            Quaternion rotationEnd = station.rotation;

            this.transform.position = Vector3.Lerp(this._positionStart, positionEnd, p);
            this.transform.rotation = Quaternion.Lerp(this._rotationStart, rotationEnd, p);
        }
    }
}