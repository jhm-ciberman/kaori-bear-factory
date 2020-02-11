using System.Collections.Generic;
using UnityEngine;

public class RequestsManager : MonoBehaviour
{
    public static LevelData currentLevelData;

    public event System.Action<Request> onActiveRequestAdded;
    public event System.Action<Request> onActiveRequestCompleted;
    public event System.Action<Request> onActiveRequestFailed;

    public event System.Action onLevelComplete;

    [SerializeField]
    public Spawner spawner;

    [SerializeField]
    public LevelData level;

    [SerializeField]
    private UIManager _uiManager = null;

    private float _nextCustomerTime = 0f;

    private RequestsQueue _queue;

    private bool levelFinished = false;

    public void Start()
    {
        if (RequestsManager.currentLevelData != null)
        {
            this.level = RequestsManager.currentLevelData;
        }
        else
        {
            RequestsManager.currentLevelData = this.level;
        }
        
        this._nextCustomerTime = Time.time + this._nextCustomerTime; 

        this._queue = new RequestsQueue(this.level.requests, this.level.slotsNumber, this.level.levelTimeMultiplier);
        this._queue.onFailRequest += this._FailRequest;

        if (this.level.requests == null || this.level.requests.Length == 0)
        {
            this._FinishLevel();
        }
    }

    public void Update()
    {
        if (this.levelFinished) return;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Q))
        {
            this._FinishLevel();
        }
#endif

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
            foreach (RequestPiece piece in activeRequest.GetPieces())
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
        this._nextCustomerTime = Time.time + this.level.customerIntervals;
    }

    private void _FinishLevel()
    {
        this.levelFinished = true;
        this.spawner.enabled = false;
        PlayerPrefs.SetInt("Level_" + this.level.name, 1);
        PlayerPrefs.Save();

        LeanTween.delayedCall(2f, () => {
            this._uiManager.ShowLevelCompleteScreen(this.level);
            this.onLevelComplete?.Invoke();
        });
    }

    private void _FailRequest(Request request)
    {
        this.onActiveRequestFailed?.Invoke(request);
        this._RemoveRequest(request);
    }

    private void _CompleteRequest(Request request)
    {
        this.onActiveRequestCompleted?.Invoke(request);
        this._RemoveRequest(request);
    }

    private void _RemoveRequest(Request request)
    {
        this._queue.RemoveActiveRequest(request);
        this.RebuildSpawnList();

        if (this._queue.levelFinished)
        {
            this._FinishLevel();
        }
        else
        {
            this._nextCustomerTime = Time.time + this.level.customerIntervals;
        }
    }

    public void DeliverCraftable(Craftable craftable, DeliveryBoxType boxType)
    {
        foreach (Request request in this._queue.activeRequests)
        {
            if (request.IsValid(craftable, boxType)) 
            {
                this._CompleteRequest(request);
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