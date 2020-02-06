using System.Collections.Generic;
using UnityEngine;

public class RequestsManager : MonoBehaviour
{
    public static LevelData currentLevelData;

    public event System.Action<ActiveRequest> onActiveRequestAdded;
    public event System.Action<ActiveRequest> onActiveRequestRemoved;

    [SerializeField]
    public Spawner spawner;

    [SerializeField]
    public LevelData level;

    [SerializeField]
    private UIManager _uiManager = null;

    private Queue<Request> _requestsQueue = new Queue<Request>();

    private System.Random _random = new System.Random();

    private List<ActiveRequest> _activeRequests = new List<ActiveRequest>();

    private float _nextCustomerTime = 0f;

    private ActiveRequest[] _slots;


    public void Start()
    {
        if (RequestsManager.currentLevelData != null)
        {
            this.level = RequestsManager.currentLevelData;
        }
        
        this._nextCustomerTime = Time.time + this._nextCustomerTime; 

        for (int i = 0; i < this.level.customers.Length; i++)
        {
            CustomerData customer = this.level.customers[i];
            Request request = customer.MakeRequest(this._random);
            this._requestsQueue.Enqueue(request);
        }

        this._slots = new ActiveRequest[this.level.slotsNumber];
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
        if (this._activeRequests.Count >= this.level.slotsNumber) return;

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
        ActiveRequest activeRequest = new ActiveRequest(request, this.level.levelTimeMultiplier);
        this._AllocateSlot(activeRequest);

        this._activeRequests.Add(activeRequest);

        this.onActiveRequestAdded?.Invoke(activeRequest);

        activeRequest.onLost += this._LoseLevel;

        this.RebuildSpawnList();
        this._nextCustomerTime = Time.time + this.level.customerIntervals;
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
                this._nextCustomerTime = Time.time + this.level.customerIntervals;
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