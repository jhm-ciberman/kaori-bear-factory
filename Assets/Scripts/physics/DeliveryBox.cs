using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DeliveryBox : MonoBehaviour
{
    public class DeliveryBoxTrigger : MonoBehaviour
    {
        public event Action<Piece> onPieceEnter;
        
        public void OnTriggerEnter(Collider other)
        {
            var pcd = other.gameObject.GetComponent<Piece.CollisionDetection>();

            if (pcd?.piece != null)
            {
                this.onPieceEnter?.Invoke(pcd.piece);
            }
        }
    }

    public event Action<CraftablePiece, DeliveryBoxType> onCraftableDelivered;

    public DeliveryBoxType type;

    public Collider trigger;

    public void Awake()
    {
        this.trigger.gameObject.AddComponent<DeliveryBoxTrigger>().onPieceEnter += this._OnPieceEnter;
    }
    
    private void _OnPieceEnter(Piece piece)
    {
        if (piece is CraftablePiece)
        {
            this.onCraftableDelivered?.Invoke(piece as CraftablePiece, this.type);
        }
                
        piece.Dispawn();
    }
}