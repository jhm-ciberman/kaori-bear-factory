using System.Collections.Generic;

public class DeliveredProduct
{
    private HashSet<RequestPiece> _set = new HashSet<RequestPiece>();

    public DeliveredProduct(CraftablePiece craftable)
    {
        this._set.Add(new RequestPiece(craftable.pieceData, PieceDirection.None, craftable.skin.data));

        foreach (AttachSpot spot in craftable.spots)
        {
            if (spot.attachedPiece != null)
            {
                this._set.Add(new RequestPiece(spot.attachedPiece.pieceData, spot.spotDirection, spot.attachedPiece.skin.data));
            }
        }
    }

    public HashSet<RequestPiece> GetMissingParts(Request request)
    {
        HashSet<RequestPiece> missingParts = new HashSet<RequestPiece>(request.pieces);
        missingParts.ExceptWith(this._set);
        return missingParts;
    }

    public bool IsValid(Request request, DeliveryBoxType boxType)
    {
        return new HashSet<RequestPiece>(request.pieces).SetEquals(this._set) && boxType == request.deliveryBoxType;
    }

    public HashSet<RequestPiece> GetExtraParts(Request request)
    {
        HashSet<RequestPiece> extraParts = new HashSet<RequestPiece>(this._set);
        extraParts.ExceptWith(request.pieces);
        return extraParts;
    }

}