using System.Collections.Generic;
using UnityEngine;

public class RequestsManager : MonoBehaviour
{
    public static LevelData currentLevelData;

    public event System.Action<Request> onActiveRequestAdded;
    public event System.Action<Request> onActiveRequestRemoved;

    [SerializeField]
    public Spawner spawner;

    [SerializeField]
    public LevelData level;

    [SerializeField]
    private UIManager _uiManager = null;

    private float _nextCustomerTime = 0f;

    private RequestsQueue _queue;

    public void Start()
    {
        if (RequestsManager.currentLevelData != null)
        {
            this.level = RequestsManager.currentLevelData;
        }
        
        this._nextCustomerTime = Time.time + this._nextCustomerTime; 

        this._queue = new RequestsQueue(this.level.customers, this.level.slotsNumber, this.level.levelTimeMultiplier);
    }

    public void Update()
    {
        this._queue.Update(Time.deltaTime);

        if (Time.time > this._nextCustomerTime)
        {
            if (! this._queue.hasFreeSlots) return;

            if (! this._queue.hasRequestsInQueue)
            {
                if (! this._queue.hasActiveRequests)
                {
                    this._WinLevel();
                }
                return;
            }

            this.DequeueNextRequest();
        }
    }

    public void RebuildSpawnList()
    {
        this.spawner.ClearSpwnList();

        foreach (var activeRequest in this._queue.activeRequests)
        {
            foreach (RequestPiece piece in activeRequest.GetPieces())
            {
                this.spawner.AddPieceToSpawnList(piece.data, piece.skin);
            }
        }

        this.spawner.ReinitSpawnList();
    }

    public void DequeueNextRequest()
    {
        Request req = this._queue.Dequeue();

        this.onActiveRequestAdded?.Invoke(req);
        req.onLost += this._LoseLevel;

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
        foreach (Request activeRequest in this._queue.activeRequests)
        {
            if (activeRequest.IsValid(craftable, boxType)) 
            {
                this._queue.RemoveActiveRequest(activeRequest);
                this.onActiveRequestRemoved?.Invoke(activeRequest);
                this.RebuildSpawnList();
                this._nextCustomerTime = Time.time + this.level.customerIntervals;
                return;
            }
        }

        // Else, the craftable is invalid! 
        Debug.Log("Invalid craftable!!");
        foreach (Request activeRequest in this._queue.activeRequests)
        {
            this._ShowErrorsForInvalidCraftable(activeRequest, craftable);
        }
    }

    private bool _ShowErrorsForInvalidCraftable(Request activeRequest, Craftable craftable)
    {
        HashSet<RequestPiece> missing = activeRequest.GetMissingParts(craftable);
        HashSet<RequestPiece> extra = activeRequest.GetExtraParts(craftable);

        Debug.Log("Client review:" + activeRequest.customer.name);

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