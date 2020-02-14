using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [System.Serializable]
    public struct Unlockable
    {
        public string name;
        public GameObject model;
    }

    [SerializeField] public string displayName = "Level";

    [SerializeField] public float levelTimeMultiplier = 1f;

    [SerializeField] public RequestData[] requests;

    [SerializeField] public int slotsNumber = 3;

    [SerializeField] public float customerIntervals = 3f;

    public Unlockable[] unlockables;
}