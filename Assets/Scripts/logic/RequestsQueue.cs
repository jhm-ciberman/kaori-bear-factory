using System.Collections.Generic;

public class RequestsQueue
{
    private Queue<Request> _requestsQueue = new Queue<Request>();

    private System.Random _random = new System.Random();

    private List<Request> _activeRequests = new List<Request>();

    private Request[] _slots;
    
    public RequestsQueue(CustomerData[] customers, int slotsNumber, float levelTimeMultiplier)
    {
        foreach (CustomerData customerData in customers)
        {
            Request request = customerData.MakeRequest(this._random, levelTimeMultiplier);
            this._requestsQueue.Enqueue(request);
        }

        this._slots = new Request[slotsNumber];
    }

    public void Update(float deltaTime)
    {
        foreach (var activeRequest in this._activeRequests)
        {
            activeRequest.Update(deltaTime);
        }
    }

    public Request Dequeue()
    {
        Request req = this._requestsQueue.Dequeue();
        this._AllocateSlot(req);

        this._activeRequests.Add(req);

        return req;
    }

    public void RemoveActiveRequest(Request activeRequest)
    {
        this._activeRequests.Remove(activeRequest);
        this._FreeSlot(activeRequest);
    }

    public IEnumerable<Request> activeRequests
    {
        get => this._activeRequests;
    }

    public bool hasFreeSlots
    {
        get => (this._activeRequests.Count < this._slots.Length);
    }

    public bool hasActiveRequests
    {
        get => (this._activeRequests.Count > 0);
    }

    public bool hasRequestsInQueue
    {
        get => (this._requestsQueue.Count > 0);
    }

    private void _AllocateSlot(Request req)
    {
        for (int i = 0; i < this._slots.Length; i++)
        {
            if (this._slots[i] == null) 
            {
                this._slots[i] = req;
                req.slot = i;
                return;
            }
        }
        throw new System.Exception("No slot available");
    }

    private void _FreeSlot(Request activeRequest)
    {
        for (int i = 0; i < this._slots.Length; i++)
        {
            if (this._slots[i] == activeRequest) 
            {
                this._slots[i] = null;
                return;
            }   
        }
    }
}