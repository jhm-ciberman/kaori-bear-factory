using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttachSpot : MonoBehaviour
{
    [HideInInspector]
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

        this.onAttachSpotEnter?.Invoke(piece, this);
    }

    protected Piece _GetPiece(Collider other)
    {
        var pcd = other.gameObject.GetComponent<Piece.CollisionDetection>();
        return pcd?.piece;
    }
}