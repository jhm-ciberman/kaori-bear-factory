using System.Collections.Generic;
using UnityEngine;

public class RequestsManager : MonoBehaviour
{
    public Spawner spawner;

    [SerializeField]
    private UIManager _uiManager;

    public CustomerData[] customers;

    private Queue<Request> _requestsQueue = new Queue<Request>();

    private System.Random _random = new System.Random();

    private List<ActiveRequest> _activeRequests = new List<ActiveRequest>();

    public int maxNumberOfAciveRequests = 3;

    public float customerIntervals = 3f;

    private float _nextCustomerTime = 0f;

    public void Start()
    {
        this._nextCustomerTime = Time.time + this._nextCustomerTime; 

        for (int i = 0; i < this.customers.Length; i++)
        {
            CustomerData customer = this.customers[i];
            Request request = customer.MakeRequest(this._random);
            this._requestsQueue.Enqueue(request);
        }
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
        if (this._activeRequests.Count >= this.maxNumberOfAciveRequests) return;

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

    public void DequeueNextRequest()
    {
        Request request = this._requestsQueue.Dequeue();
        ActiveRequest activeRequest = new ActiveRequest(request);
        this._activeRequests.Add(activeRequest);
        this._uiManager.AddActiveRequest(activeRequest);

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
                this._uiManager.RemoveActiveRequest(activeRequest);
                this.RebuildSpawnList();
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

        if (missing.Count == 0 && extra.Count == 0)
        {
            return true;
        }

        return false;
    }
}