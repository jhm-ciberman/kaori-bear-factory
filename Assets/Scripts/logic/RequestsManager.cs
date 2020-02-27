using System.Collections.Generic;
using UnityEngine;

public class RequestsManager : MonoBehaviour
{
    public event System.Action<Request> onActiveRequestAdded;
    public event System.Action<Request> onActiveRequestCompleted;
    public event System.Action<Request> onActiveRequestFailed;
    public event System.Action<LevelData> onLevelComplete;

    [SerializeField] public Spawner spawner = null;

    [HideInInspector] private LevelData _level = null;

    private float _nextCustomerTime = 0f;

    private RequestsQueue _queue;

    public void StartLevel(LevelData level)
    {
        this._level = level;
        this._nextCustomerTime = Time.time + this._nextCustomerTime; 

        this._queue = new RequestsQueue(this._level, this._level.slotsNumber, this._level.levelTimeMultiplier);
        this._queue.onFailRequest += this._RemoveRequest;

        if (this._level.requests == null || this._level.requests.Length == 0)
        {
            this.CompleteLevel();
        }
    }

    public int requestsInQueueCount
    {
        get => this._queue.count;
    }

    public int requestsActiveCount
    {
        get => this._queue.activeRequestsCount;
    }

    public void Update()
    {
        if (this._level == null) return;

        this._queue.Update(Time.deltaTime);

        if (Time.time > this._nextCustomerTime)
        {
            if (this._queue.levelFinished) return;
            if (! this._queue.hasFreeSlots) return;

            if (this._queue.hasRequestsInQueue)
            {
                this._AddNextRequest();
            }
        }
    }

    public void RebuildSpawnList()
    {
        this.spawner.ClearSpwnList();

        foreach (var activeRequest in this._queue.activeRequests)
        {
            foreach (RequestPiece piece in activeRequest.pieces)
            {
                this.spawner.AddPieceToSpawnList(piece.data, piece.skin);
            }
        }

        this.spawner.ReinitSpawnList();
    }

    public void _AddNextRequest()
    {
        Request req = this._queue.Dequeue();

        this.onActiveRequestAdded?.Invoke(req);

        this.RebuildSpawnList();
        this._nextCustomerTime = Time.time + this._level.customerIntervals;
    }

    public void CompleteLevel()
    {
        this.onLevelComplete?.Invoke(this._level);
        this._level = null;
        this.spawner.enabled = false;
    }

    private void _RemoveRequest(Request request)
    {
        this._queue.RemoveActiveRequest(request);
        
        if (request.failed)
        {
            this.onActiveRequestFailed?.Invoke(request);
        }
        else 
        {
            this.onActiveRequestCompleted?.Invoke(request);
        }

        this.RebuildSpawnList();

        if (this._queue.levelFinished)
        {
            this.CompleteLevel();
        }
        else
        {
            this._nextCustomerTime = Time.time + this._level.customerIntervals;
        }
    }

    public void DeliverCraftable(CraftablePiece craftable, DeliveryBoxType boxType)
    {
        DeliveredProduct delivered = new DeliveredProduct(craftable);

        foreach (Request request in this._queue.activeRequests)
        {
            if (delivered.IsValid(request, boxType)) 
            {
                this._RemoveRequest(request);
                return;
            }
        }

        // Else, the craftable is invalid! 
        Debug.Log("Invalid craftable!!");
        foreach (Request activeRequest in this._queue.activeRequests)
        {
            this._ShowErrorsForInvalidCraftable(activeRequest, delivered);
        }
    }

    private bool _ShowErrorsForInvalidCraftable(Request activeRequest, DeliveredProduct delivered)
    {
        HashSet<RequestPiece> missing = delivered.GetMissingParts(activeRequest);
        HashSet<RequestPiece> extra = delivered.GetExtraParts(activeRequest);

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