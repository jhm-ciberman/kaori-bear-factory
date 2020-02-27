using System.Collections.Generic;
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

    public SkinData[] skins;

    public PieceData   body;
    public PieceData[] leftArm;
    public PieceData[] rightArm;
    public PieceData[] leftEye;
    public PieceData[] rightEye;
    public PieceData[] leftLeg;
    public PieceData[] rightLeg;
    public PieceData[] leftEar;
    public PieceData[] rightEar;
    public PieceData[] hat;
    public PieceData[] clothe;

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