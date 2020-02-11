using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Game/CustomerData", order = 1)]
public class CustomerData : ScriptableObject
{
    public Sprite customerPortrait;

    public Sprite customerAngryPortrait;

    public float patienceTime = 40f;

    public DeliveryBoxType[] deliveryBoxTypes = new DeliveryBoxType[] {
        DeliveryBoxType.Cardboard,
    };
}