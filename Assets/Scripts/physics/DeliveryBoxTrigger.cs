using UnityEngine;

[DisallowMultipleComponent]
public class DeliveryBoxTrigger : MonoBehaviour
{
    public RequestsManager requestsManager;

    public DeliveryBoxType type;

    public void OnTriggerEnter(Collider other)
    {
        var pcd = other.gameObject.GetComponent<Piece.CollisionDetection>();

        if (pcd?.piece != null)
        {
            if (pcd.piece is CraftablePiece)
            {
                this.requestsManager.DeliverCraftable(pcd.piece as CraftablePiece, this.type);
            }

            pcd.piece.Dispawn();
            return;
        }
    }
}