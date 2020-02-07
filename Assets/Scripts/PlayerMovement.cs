using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Transform[] stations;

    public void SetNormalizedValue(float value)
    {
        this.SetValue(value * (this.stations.Length - 1));
    }

    public void SetNormalizedValue(Vector2 value)
    {
        this.SetValue(value.x * (this.stations.Length - 1));
    }

    public void SetValue(float value)
    {
        if (value < 1f)
        {
            this._SetCamera(0, 1, value);
            return;
        }

        if (value > this.stations.Length - 2)
        {
            this._SetCamera(this.stations.Length - 2, this.stations.Length - 1, value - (this.stations.Length - 2));
            return;
        }

        int a = Mathf.FloorToInt(value);
        int b = Mathf.CeilToInt(value);
        this._SetCamera(a, b, value - b);
    }

    protected void _SetCamera(int a, int b, float t)
    {
        Transform stationA = this.stations[a];
        Transform stationB = this.stations[b];
        this.transform.position = Vector3.LerpUnclamped   (stationA.position, stationB.position, t);
        this.transform.rotation = Quaternion.LerpUnclamped(stationA.rotation, stationB.rotation, t);
    }
}