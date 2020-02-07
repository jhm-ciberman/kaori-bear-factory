using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttachSpot : MonoBehaviour
{
    public Piece attachedPiece = null;

    public PieceType spotType;

    public PieceDirection spotDirection;

    public delegate void OnAttachSpotEnter(Piece piece, AttachSpot spot);
    public event OnAttachSpotEnter onAttachSpotEnter;

    void OnTriggerEnter(Collider other)
    {
        if (this.attachedPiece != null) return;

        Piece piece = this._GetPiece(other);

        if (piece == null) return;
        if (piece.pieceData.type != this.spotType) return;

        if (this.onAttachSpotEnter != null)
        {
            this.onAttachSpotEnter(piece, this);
        }
    }

    protected Piece _GetPiece(Collider other)
    {
        Piece.PieceCollisionDetection dcd = other.gameObject.GetComponent<Piece.PieceCollisionDetection>();
        if (dcd != null) return dcd.piece;

        Piece.PieceHitbox ph = other.gameObject.GetComponent<Piece.PieceHitbox>();
        if (ph != null) return ph.piece;

        return null;
    }
}