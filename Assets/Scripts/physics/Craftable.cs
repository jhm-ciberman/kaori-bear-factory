using UnityEngine;
using System.Collections.Generic;

public class Craftable : MonoBehaviour
{
    public delegate void OnPieceAttached(Craftable craftable);
    public static event OnPieceAttached onPieceAttached;

    private Piece _piece;
    public Piece piece { get => this._piece; }

    public AttachSpot[] spots;

    private Piece[] _attachedPieces;

    public HashSet<RequestPiece> requestPieces = new HashSet<RequestPiece>();

    void Start()
    {
        this._piece = this.GetComponent<Piece>();

        foreach (AttachSpot spot in this.spots)
        {
            spot.onAttachSpotEnter += this.OnAttachSpotEnter;
        }

        this.requestPieces.Add(new RequestPiece(this._piece.pieceData, PieceDirection.None, this._piece.skin));
    }

    public void OnAttachSpotEnter(Piece piece, AttachSpot spot)
    {
        if (! piece.canBeAttached) return;
        
        RequestPiece request = new RequestPiece(piece.pieceData, spot.spotDirection, piece.skin);

        if (! this.requestPieces.Contains(request))
        {
            piece.Attach(this, spot.spotDirection);
            spot.attachedPiece = piece;
            this.requestPieces.Add(request);

            if (Craftable.onPieceAttached != null)
            {
                Craftable.onPieceAttached(this);
            }
        }
    }


}