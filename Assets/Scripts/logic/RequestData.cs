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
        public PieceData[] pieces;

        public RequestPiecePool(PieceDirection direction, PieceData[] pieces)
        {
            this.direction = direction;
            this.pieces = pieces;
        }
    }

    public CustomerData customer;

    [ReorderableList] public SkinData[] skins;

    public PieceData   body;
    
    [Space]

    [ReorderableList] public PieceData[] arms   = new PieceData[0];
    [ReorderableList] public PieceData[] eyes   = new PieceData[0];
    [ReorderableList] public PieceData[] legs   = new PieceData[0];
    [ReorderableList] public PieceData[] ears   = new PieceData[0];
    [ReorderableList] public PieceData[] hat    = new PieceData[0];
    [ReorderableList] public PieceData[] clothe = new PieceData[0];

    public bool perPartSkin = true;

    public IEnumerable<RequestPiecePool> pieceDataPools
    {
        get 
        {
            yield return new RequestPiecePool(PieceDirection.None , new PieceData[] {this.body});
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