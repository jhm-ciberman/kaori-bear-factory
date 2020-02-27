using System.Collections.Generic;
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

    [SerializeField] public int seed = 0;
    
    [Range(0f, 2f)] public float levelTimeMultiplier = 1f;

    [Range(0f, 30f)] public float customerIntervals = 3f;

    [SerializeField] public int slotsNumber = 3;

    [SerializeField] public RequestData[] requests;

    public Unlockable[] unlockables;

    public IEnumerable<Request> GetRequests()
    {
        Request[] requests = new Request[this.requests.Length];

        System.Random random = (this.seed == 0) ? new System.Random() : new System.Random(this.seed);

        int i = 0;
        foreach (var req in this.requests)
        {
            requests[i++] = req.MakeRequest(random, this.levelTimeMultiplier);
        }

        ListUtils.Shuffle(random, requests);

        return requests;
    }
}