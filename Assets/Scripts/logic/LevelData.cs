using System.Collections.Generic;
using System.Linq;
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

    public bool paintMachineUnlocked = true;
    public bool giftBoxUnlocked = true;

    public SkinData[] availableSkins = new SkinData[0];
    public PieceData[] availablePieces = new PieceData[0];

    public IEnumerable<Request> GetRequests()
    {
        if (this.availableSkins.Length == 0 || this.availablePieces.Length == 0) return Enumerable.Empty<Request>();
        
        System.Random random = (this.seed == 0) ? new System.Random() : new System.Random(this.seed);

        var requests = this.requests.Select(requestData => this._MakeRequest(requestData, random, this.levelTimeMultiplier));

        ListUtils.Shuffle(random, requests.ToList());

        return requests;
    }

    public SkinData defaultSkin => this.availableSkins.First();

    private T[]  _Filter<T>(T[] requestList, T[] filterList, T defaultValue)
    {
        IEnumerable<T> skins = filterList.Intersect(requestList);
        return skins.Any() ? skins.ToArray() : new T[] {defaultValue}; 
    }

    private Request _MakeRequest(RequestData requestData, System.Random random, float levelTimeMultiplier) 
    {
        Request request = new Request(requestData.customer, levelTimeMultiplier);

        var skins = this.paintMachineUnlocked 
            ? this._Filter(requestData.skins, this.availableSkins, this.defaultSkin)
            : new SkinData[] {this.defaultSkin};

        SkinData globalSkin = this._Choose(random, skins);

        foreach (var pool in requestData.pieceDataPools)
        {
            var availablePieces = this._Filter(pool.pieces, this.availablePieces, null);
            PieceData pieceData = this._Choose(random, availablePieces);

            if (pieceData == null) continue;

            SkinData skin = pieceData.skinable 
                ? (requestData.perPartSkin) ? this._Choose(random, skins) : globalSkin
                : null;

            request.AddPiece(new RequestPiece(pieceData, pool.direction, skin));
        }
        
        request.deliveryBoxType = this._ChooseDeliveryBox(random, requestData.customer.deliveryBoxTypes);

        return request;
    }

    private DeliveryBoxType _ChooseDeliveryBox(System.Random random, DeliveryBoxType[] availableDeliveryBoxes)
    {
        var box = this._Choose(random, availableDeliveryBoxes );
        return (box == DeliveryBoxType.Gift && ! this.giftBoxUnlocked) ? DeliveryBoxType.Cardboard : box;
    }

    private T _Choose<T>(System.Random random, T[] pool, T defaultValue = default(T))
    {
        if (pool.Length > 0)
            return pool[random.Next(pool.Length)];

        return defaultValue;
    }

}