using UnityEngine;
using System.Collections.Generic;

[DisallowMultipleComponent]
public class CraftablePiece : Piece
{
    public AttachSpot[] spots;

    public HashSet<RequestPiece> requestPieces = new HashSet<RequestPiece>();

    private List<Piece> _attachedPieces = new List<Piece>();

    new void Start()
    {
        base.Start();

        foreach (AttachSpot spot in this.spots)
        {
            spot.onAttachSpotEnter += this.OnAttachSpotEnter;
        }

        this.requestPieces.Add(new RequestPiece(this.pieceData, PieceDirection.None, this.skin));
    }

    public IEnumerable<Piece> attachedPieces
    {
        get => this._attachedPieces;
    }

    public void OnAttachSpotEnter(Piece piece, AttachSpot spot)
    {
        if (piece.isDragged) return;
        
        RequestPiece request = new RequestPiece(piece.pieceData, spot.spotDirection, piece.skin);

        if (! this.requestPieces.Contains(request))
        {
            piece.Attach(this, spot.spotDirection);
            piece.draggable = false;
            spot.attachedPiece = piece;
            this.requestPieces.Add(request);
            this._attachedPieces.Add(piece);
        }
    }
}