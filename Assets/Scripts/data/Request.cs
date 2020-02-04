using System.Collections.Generic;
using UnityEngine;

public class Request
{
    public CustomerData customer;

    public Request(CustomerData customer)
    {
        this.customer = customer;
    }

    private HashSet<RequestPiece> _pieces = new HashSet<RequestPiece>();

    public DeliveryBoxType deliveryBoxType;

    public float maximumTime;

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

}