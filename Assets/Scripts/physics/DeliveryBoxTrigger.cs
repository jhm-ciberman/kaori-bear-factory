using UnityEngine;

public class DeliveryBoxTrigger : MonoBehaviour
{
    public RequestsManager requestsManager;

    public DeliveryBoxType type;

    public void OnTriggerEnter(Collider other)
    {
        CraftableCollisionDetection ccd = other.gameObject.GetComponent<CraftableCollisionDetection>();

        if (ccd != null)
        {
            ccd.craftable.piece.Dispawn();

            this.requestsManager.DeliverCraftable(ccd.craftable, this.type);
            return;
        }

        Piece.PieceCollisionDetection pcd = other.gameObject.GetComponent<Piece.PieceCollisionDetection>();

        if (pcd != null)
        {
            pcd.piece.Dispawn();
            return;
        }
    }
}