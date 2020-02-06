using System.Collections.Generic;
using UnityEngine;

public class RequestsManager : MonoBehaviour
{
    public event System.Action<ActiveRequest> onActiveRequestAdded;
    public event System.Action<ActiveRequest> onActiveRequestRemoved;

    [SerializeField]
    public Spawner spawner;

    [SerializeField]
    private UIManager _uiManager = null;

    public CustomerData[] customers;

    private Queue<Request> _requestsQueue = new Queue<Request>();

    private System.Random _random = new System.Random();

    private List<ActiveRequest> _activeRequests = new List<ActiveRequest>();

    public int slotsNumber = 3;

    public float customerIntervals = 3f;

    private float _nextCustomerTime = 0f;

    private ActiveRequest[] _slots;

    public void Start()
    {
        this._nextCustomerTime = Time.time + this._nextCustomerTime; 

        for (int i = 0; i < this.customers.Length; i++)
        {
            CustomerData customer = this.customers[i];
            Request request = customer.MakeRequest(this._random);
            this._requestsQueue.Enqueue(request);
        }

        this._slots = new ActiveRequest[this.slotsNumber];
    }

    public void Update()
    {
        foreach (var activeRequest in this._activeRequests)
        {
            activeRequest.Update(Time.deltaTime);
        }

        if (Time.time > this._nextCustomerTime)
        {
            this.TryToFitNewCustomer();
        }
    }

    public void TryToFitNewCustomer()
    {
        if (this._activeRequests.Count >= this.slotsNumber) return;

        if (this._requestsQueue.Count == 0)
        {
            if (this._activeRequests.Count == 0)
            {
                this._WinLevel();
            }
            return;
        }

        this.DequeueNextRequest();
    }

    public void RebuildSpawnList()
    {
        this.spawner.ClearSpwnList();

        foreach (var activeRequest in this._activeRequests)
        {
            foreach (RequestPiece piece in activeRequest.request.GetPieces())
            {
                this.spawner.AddPieceToSpawnList(piece.data, piece.skin);
            }
        }
    }

    private void _AllocateSlot(ActiveRequest activeRequest)
    {
        for (int i = 0; i < this._slots.Length; i++)
        {
            if (this._slots[i] == null) 
            {
                this._slots[i] = activeRequest;
                activeRequest.slot = i;
                return;
            }
        }
        throw new System.Exception("No slot available");
    }

    private void _FreeSlot(ActiveRequest activeRequest)
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

    public void DequeueNextRequest()
    {
        Request request = this._requestsQueue.Dequeue();
        ActiveRequest activeRequest = new ActiveRequest(request);
        this._AllocateSlot(activeRequest);

        this._activeRequests.Add(activeRequest);

        this.onActiveRequestAdded?.Invoke(activeRequest);

        activeRequest.onLost += this._LoseLevel;

        this.RebuildSpawnList();
        this._nextCustomerTime = Time.time + this.customerIntervals;
    }

    private void _WinLevel()
    {
        this._uiManager.ShowWinScreen();
        Debug.Log("You wiiiin!");
    }

    private void _LoseLevel()
    {
        Debug.Log("You loose!");
    }

    public void DeliverCraftable(Craftable craftable, DeliveryBoxType boxType)
    {
        foreach (ActiveRequest activeRequest in this._activeRequests)
        {
            if (activeRequest.request.IsValid(craftable, boxType)) 
            {
                this._activeRequests.Remove(activeRequest);
                this._FreeSlot(activeRequest);
                this.onActiveRequestRemoved?.Invoke(activeRequest);
                this.RebuildSpawnList();
                this._nextCustomerTime = Time.time + this.customerIntervals;
                return;
            }
        }

        // Else, the craftable is invalid! 
        Debug.Log("Invalid craftable!!");
        foreach (ActiveRequest activeRequest in this._activeRequests)
        {
            this._ShowErrorsForInvalidCraftable(activeRequest, craftable);
        }
    }

    private bool _ShowErrorsForInvalidCraftable(ActiveRequest activeRequest, Craftable craftable)
    {
        HashSet<RequestPiece> missing = activeRequest.request.GetMissingParts(craftable);
        HashSet<RequestPiece> extra = activeRequest.request.GetExtraParts(craftable);

        Debug.Log("Client review:" + activeRequest.request.customer.name);

        foreach (RequestPiece part in missing)
        {
            Debug.Log("Missing: " + part.data.name + " " + part.direction.ToString() + " " + part.skin?.name);
        }

        foreach (RequestPiece part in extra)
        {
            Debug.Log("Extra: " + part.data.name + " " + part.direction.ToString() + " " + part.skin?.name);
        }

        return (missing.Count == 0 && extra.Count == 0);
    }
}