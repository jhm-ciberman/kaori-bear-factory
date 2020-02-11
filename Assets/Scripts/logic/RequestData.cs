using UnityEngine;

[CreateAssetMenu(fileName = "RequestData", menuName = "Game/RequestData", order = 1)]
public class RequestData : ScriptableObject
{
    [System.Serializable]
    public struct RequestPiecePool
    {
        public PieceDirection direction;
        public PieceData[] pieces;
    }

    public CustomerData customer;

    public PieceSkin[] skins;

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

    public Request MakeRequest(System.Random random, float levelTimeMultiplier) 
    {
        Request request = new Request(this.customer, levelTimeMultiplier);
        
        PieceSkin globalSkin = this.skins[random.Next(this.skins.Length)];

        request.AddPiece(new RequestPiece(this.body, PieceDirection.None, this.body.skinable ? globalSkin : null));

        this._AddPiece(request, random, this.leftArm      , PieceDirection.Left  , globalSkin);
        this._AddPiece(request, random, this.rightArm     , PieceDirection.Right , globalSkin);
        this._AddPiece(request, random, this.leftEye      , PieceDirection.Left  , globalSkin);
        this._AddPiece(request, random, this.rightEye     , PieceDirection.Right , globalSkin);
        this._AddPiece(request, random, this.leftLeg      , PieceDirection.Left  , globalSkin);
        this._AddPiece(request, random, this.rightLeg     , PieceDirection.Right , globalSkin);
        this._AddPiece(request, random, this.leftEar      , PieceDirection.Left  , globalSkin);
        this._AddPiece(request, random, this.rightEar     , PieceDirection.Right , globalSkin);
        this._AddPiece(request, random, this.hat          , PieceDirection.None  , globalSkin);
        this._AddPiece(request, random, this.clothe       , PieceDirection.None  , globalSkin);

        request.deliveryBoxType = this.customer.deliveryBoxTypes[random.Next(this.customer.deliveryBoxTypes.Length)];
        return request;
    }

    private void _AddPiece(Request request, System.Random random, PieceData[] pool, PieceDirection direction, PieceSkin globalSkin)
    {
        if (pool.Length == 0) return;

        PieceData pieceData = pool[random.Next(pool.Length)];

        if (pieceData == null) return;

        PieceSkin skin = (this.perPartSkin) ? this.skins[random.Next(this.skins.Length)] : globalSkin;

        request.AddPiece(new RequestPiece(pieceData, direction, pieceData.skinable ? skin : null));
    }

}