using System.Collections.Generic;
using UnityEngine;

public class Request
{
    public CustomerData customer;

    public delegate void OnLost();
    public event OnLost onLost;

    public bool lost = false;

    public int slot = 0;

    public float elapsedTime = 0f;
    public float maximumTime;

    private HashSet<RequestPiece> _pieces = new HashSet<RequestPiece>();

    public DeliveryBoxType deliveryBoxType;
    
    public Request(CustomerData customer, float maximumTime)
    {
        this.customer = customer;
        this.maximumTime = maximumTime;
    }



    public HashSet<RequestPiece> GetMissingParts(Craftable craftable)
    {
        HashSet<RequestPiece> missingParts = new HashSet<RequestPiece>(this._pieces);
        missingParts.ExceptWith(craftable.requestPieces);
        return missingParts;
    }

    public bool IsValid(Craftable craftable, DeliveryBoxType boxType)
    {
        return this._pieces.SetEquals(craftable.requestPieces) && boxType == this.deliveryBoxType;
    }

    public HashSet<RequestPiece> GetExtraParts(Craftable craftable)
    {
        HashSet<RequestPiece> extraParts = new HashSet<RequestPiece>(craftable.requestPieces);
        extraParts.ExceptWith(this._pieces);
        return extraParts;
    }

    public IEnumerable<RequestPiece> GetPieces()
    {
        return this._pieces;
    }

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