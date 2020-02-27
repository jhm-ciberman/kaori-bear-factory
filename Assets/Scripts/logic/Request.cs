using System.Collections.Generic;
using UnityEngine;

public class Request
{
    public CustomerData customer;

    public bool failed = false;

    public int slot = 0;

    public float elapsedTime = 0f;
    public float maximumTime;

    private HashSet<RequestPiece> _pieces = new HashSet<RequestPiece>();

    public DeliveryBoxType deliveryBoxType;
    
    public Request(CustomerData customer, float levelTimeMultiplier)
    {
        this.customer = customer;
        this.maximumTime = levelTimeMultiplier * this.customer.patienceTime;
    }

    public IEnumerable<RequestPiece> pieces => this._pieces;

    public bool CanAddPiece(RequestPiece requestPiece)
    {
        return ! this._pieces.Contains(requestPiece);
    }

    public bool AddPiece(RequestPiece requestPiece)
    {
        return this._pieces.Add(requestPiece);
    }

    public float progress
    {
        get => 1f - (this.elapsedTime / this.maximumTime);
    }

}