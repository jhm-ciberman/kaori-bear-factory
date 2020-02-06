using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    public float levelTimeMultiplier = 1f;

    [SerializeField]
    public CustomerData[] customers;

    [SerializeField]
    public int slotsNumber = 3;

    [SerializeField]
    public float customerIntervals = 3f;
}