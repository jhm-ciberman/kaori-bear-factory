using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AttachSpot : MonoBehaviour
{
    public PieceType spotType;

    public PieceDirection spotDirection;

    public delegate void OnAttachSpotEnter(Piece piece, AttachSpot spot);
    public event OnAttachSpotEnter onAttachSpotEnter;

    void OnTriggerEnter(Collider other)
    {
        if (! this.enabled) return;

        Piece.PieceCollisionDetection dcd = other.gameObject.GetComponent<Piece.PieceCollisionDetection>();
        if (dcd == null) return;

        if (dcd.piece == null) return;
        if (dcd.piece.pieceData.type != this.spotType) return;

        if (this.onAttachSpotEnter != null)
        {
            this.onAttachSpotEnter(dcd.piece, this);
        }
    }
}