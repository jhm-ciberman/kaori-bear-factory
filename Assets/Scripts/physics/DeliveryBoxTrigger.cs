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
            Object.Destroy(ccd.craftable.gameObject);

            this.requestsManager.DeliverCraftable(ccd.craftable, this.type);
            return;
        }

        Piece.PieceCollisionDetection pcd = other.gameObject.GetComponent<Piece.PieceCollisionDetection>();

        if (pcd != null)
        {
            Object.Destroy(pcd.gameObject);
            return;
        }
    }
}