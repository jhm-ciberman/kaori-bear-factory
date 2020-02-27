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

    [ReorderableList] public PieceData[] leftArm;
    [ReorderableList] public PieceData[] rightArm;
    [ReorderableList] public PieceData[] leftEye;
    [ReorderableList] public PieceData[] rightEye;
    [ReorderableList] public PieceData[] leftLeg;
    [ReorderableList] public PieceData[] rightLeg;
    [ReorderableList] public PieceData[] leftEar;
    [ReorderableList] public PieceData[] rightEar;
    [ReorderableList] public PieceData[] hat;
    [ReorderableList] public PieceData[] clothe;

    public bool perPartSkin = true;

    public IEnumerable<RequestPiecePool> pieceDataPools
    {
        get 
        {
            yield return new RequestPiecePool(PieceDirection.None , new PieceData[] {this.body});
            yield return new RequestPiecePool(PieceDirection.Left , this.leftArm );
            yield return new RequestPiecePool(PieceDirection.Right, this.rightArm);
            yield return new RequestPiecePool(PieceDirection.Left , this.leftEye );
            yield return new RequestPiecePool(PieceDirection.Right, this.rightEye);
            yield return new RequestPiecePool(PieceDirection.Left , this.leftLeg );
            yield return new RequestPiecePool(PieceDirection.Right, this.rightLeg);
            yield return new RequestPiecePool(PieceDirection.Left , this.leftEar );
            yield return new RequestPiecePool(PieceDirection.Right, this.rightEar);
            yield return new RequestPiecePool(PieceDirection.None , this.hat     );
            yield return new RequestPiecePool(PieceDirection.None , this.clothe  );
        }
    }
}