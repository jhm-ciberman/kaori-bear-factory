public class ActiveRequest
{
    public Request request;

    public delegate void OnLost();
    public event OnLost onLost;

    public bool lost = false;

    public ActiveRequest(Request request)
    {
        this.request = request;
    }

    public float elapsedTime = 0f;

    public void Update(float deltaTime)
    {
        if (this.lost) return;
        this.elapsedTime += deltaTime;

        if (this.elapsedTime >= this.request.maximumTime)
        {
            this.elapsedTime = this.request.maximumTime;
            this.lost = true;
            if (this.onLost != null) this.onLost();
        }
    }
}