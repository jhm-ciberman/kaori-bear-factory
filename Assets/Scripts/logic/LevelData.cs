using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Game/LevelData", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] public string displayName = "Level";

    [SerializeField] public int seed = 0;
    
    [Range(0f, 2f)] public float levelTimeMultiplier = 1f;

    [Range(0f, 4f)] public float paintingTimePerPiece = 3f;

    [Range(0f, 30f)] public float customerIntervals = 3f;

    [SerializeField] public int slotsNumber = 3;

    [ReorderableList] public RequestData[] requests;

    [System.Serializable]
    public struct Unlockable
    {
        public string name;
        public GameObject model;
    }

    [ReorderableList] public Unlockable[] afterLevelCompleteUnlockables = new Unlockable[0];

    public bool giftBoxUnlocked = true;

    [ReorderableList] public SkinData[] availableSkins = new SkinData[0];
    [ReorderableList] public PieceData[] availablePieces = new PieceData[0];

    public GameObject tutorialUI = null;

    public IEnumerable<Request> GetRequests()
    {
        if (this.availableSkins.Length == 0 || this.availablePieces.Length == 0) return Enumerable.Empty<Request>();
        
        System.Random random = (this.seed == 0) ? new System.Random() : new System.Random(this.seed);

        var requests = this.requests.Select(requestData => this._MakeRequest(requestData, random, this.levelTimeMultiplier));

        requests = ListUtils.Shuffle(random, requests.ToList());

        return requests;
    }

    private Request _MakeRequest(RequestData requestData, System.Random random, float levelTimeMultiplier) 
    {
        Request request = new Request(requestData.customer, levelTimeMultiplier);

        var skins = this._GetAvailableSkins(random, requestData);
        SkinData globalSkin = skins.Next();

        foreach (var pool in requestData.pieceDataPools)
        {
            var pieceData = this._ChoosePiece(random, pool.pieces);

            if (pieceData == null) continue;

            SkinData skin = pieceData.skinable 
                ? (requestData.perPartSkin) ? skins.Next() : globalSkin
                : null;

            request.AddPiece(new RequestPiece(pieceData, pool.direction, skin));
        }
        
        request.deliveryBoxType = this._ChooseDeliveryBox(random, requestData.customer.deliveryBoxTypes);

        return request;
    }

    public WeightedRandom<SkinData> _GetAvailableSkins(System.Random random, RequestData requestData)
    {
        var wr = new WeightedRandom<SkinData>(random);

        if (this.availableSkins.Length < 2)
        {
            wr.Add(this.availableSkins.First(), 1f);
        }
        else
        {
            foreach (var weigtedSkin in requestData.skins)
            {
                if (this.availableSkins.Contains(weigtedSkin.skin))
                {
                    wr.Add(weigtedSkin.skin, weigtedSkin.probability);
                }
            }
        }

        return wr;
    }

    public PieceData _ChoosePiece(System.Random random, RequestData.WeigtedPieceData[] pool)
    {
        var wr = new WeightedRandom<PieceData>(random);

        foreach (var weigtedPiece in pool)
        {
            if (weigtedPiece.piece == null || this.availablePieces.Contains(weigtedPiece.piece))
            {
                wr.Add(weigtedPiece.piece, weigtedPiece.probability);
            }
        }

        return wr.Next();;
    }


    private DeliveryBoxType _ChooseDeliveryBox(System.Random random, DeliveryBoxType[] availableDeliveryBoxes)
    {
        var box = availableDeliveryBoxes[random.Next(availableDeliveryBoxes.Length)];
        return (box == DeliveryBoxType.Gift && ! this.giftBoxUnlocked) ? DeliveryBoxType.Cardboard : box;
    }
}