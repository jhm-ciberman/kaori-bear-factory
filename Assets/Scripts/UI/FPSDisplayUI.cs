using UnityEngine;

public class FPSDisplayUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _fpsText;
    [SerializeField] private float _hudRefreshRate = 1f;

    private float _timer;

    private void Update()
    {
        if (Time.unscaledTime > _timer)
        {
            int fps = (int)(1f / Time.unscaledDeltaTime);
            this._fpsText.text = "FPS: " + fps;
            this._timer = Time.unscaledTime + this._hudRefreshRate;
        }
    }
}