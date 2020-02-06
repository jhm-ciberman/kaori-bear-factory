public class ActiveRequest
{
    public Request request;

    public delegate void OnLost();
    public event OnLost onLost;

    public bool lost = false;

    public int slot = 0;

    public float elapsedTime = 0f;
    public float maximumTime;

    public ActiveRequest(Request request, float levelTimeMultiplier)
    {
        this.request = request;
        this.maximumTime = request.maximumTime * levelTimeMultiplier;
    }

    public float progress
    {
        get => 1f - (this.elapsedTime / this.maximumTime);
    }

    public void Update(float deltaTime)
    {
        if (this.lost) return;
        this.elapsedTime += deltaTime;

        if (this.elapsedTime >= this.maximumTime)
        {
            this.elapsedTime = this.maximumTime;
            this.lost = true;
            if (this.onLost != null) this.onLost();
        }
    }
}