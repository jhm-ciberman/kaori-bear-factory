using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "RequestData", menuName = "Game/RequestData", order = 1)]
public class RequestData : ScriptableObject
{
    [System.Serializable]
    public struct RequestPiecePool
    {
        public PieceDirection direction;
        public WeigtedPieceData[] pieces;

        public RequestPiecePool(PieceDirection direction, WeigtedPieceData[] pieces)
        {
            this.direction = direction;
            this.pieces = pieces;
        }
    }

    [System.Serializable]
    public struct WeigtedPieceData
    {
        public PieceData piece;
        
        [Range(0,1)] public float probability;

        public WeigtedPieceData(PieceData piece, float probability)
        {
            this.piece = piece;
            this.probability = probability;
        }

    }

    [System.Serializable]
    public struct WeigtedSkinData
    {
        public SkinData skin;
        [Range(0,1)] public float probability;
    }

    [Required] public CustomerData customer;

    [ReorderableList] public WeigtedSkinData[] skins = new WeigtedSkinData[0];
    public bool perPartSkin = true;

    [Required] public PieceData   body;
    
    [Space]

    [ReorderableList] public WeigtedPieceData[] arms   = new WeigtedPieceData[0];
    [ReorderableList] public WeigtedPieceData[] eyes   = new WeigtedPieceData[0];
    [ReorderableList] public WeigtedPieceData[] legs   = new WeigtedPieceData[0];
    [ReorderableList] public WeigtedPieceData[] ears   = new WeigtedPieceData[0];
    [ReorderableList] public WeigtedPieceData[] hat    = new WeigtedPieceData[0];
    [ReorderableList] public WeigtedPieceData[] clothe = new WeigtedPieceData[0];



    public IEnumerable<RequestPiecePool> pieceDataPools
    {
        get 
        {
            yield return new RequestPiecePool(PieceDirection.None , 
                new WeigtedPieceData[] {new WeigtedPieceData(this.body, 1.0f)}
            );
            yield return new RequestPiecePool(PieceDirection.Left , this.arms);
            yield return new RequestPiecePool(PieceDirection.Right, this.arms);
            yield return new RequestPiecePool(PieceDirection.Left , this.eyes);
            yield return new RequestPiecePool(PieceDirection.Right, this.eyes);
            yield return new RequestPiecePool(PieceDirection.Left , this.legs);
            yield return new RequestPiecePool(PieceDirection.Right, this.legs);
            yield return new RequestPiecePool(PieceDirection.Left , this.ears);
            yield return new RequestPiecePool(PieceDirection.Right, this.ears);
            yield return new RequestPiecePool(PieceDirection.None , this.hat);
            yield return new RequestPiecePool(PieceDirection.None , this.clothe);
        }
    }
}