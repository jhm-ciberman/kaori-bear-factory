using UnityEngine;

public class DeliveryBoxTrigger : MonoBehaviour
{
    public RequestsManager requestsManager;

    public DeliveryBoxType type;

    public void OnCollisionEnter(Collision other)
    {
        CraftableCollisionDetection ccd = other.gameObject.GetComponent<CraftableCollisionDetection>();

        if (ccd != null)
        {
            Object.Destroy(ccd.craftable.gameObject);

            this.requestsManager.DeliverCraftable(ccd.craftable, this.type);
        }
    }
}