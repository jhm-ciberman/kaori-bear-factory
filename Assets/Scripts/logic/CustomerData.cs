using UnityEngine;

[CreateAssetMenu(fileName = "CustomerData", menuName = "Game/CustomerData", order = 1)]
public class CustomerData : ScriptableObject
{
    [System.Serializable]
    public struct RequestPiecePool
    {
        public RequestPiece[] pieces;
    }

    public Sprite customerPortrait;

    public Sprite customerAngryPortrait;

    public float patienceTime = 40f;

    public PieceSkin[] skins;

    public RequestPiece[] compulsoryParts;

    public RequestPiecePool[] optionalPartsPools;

    public bool perPartSkin = true;

    public DeliveryBoxType[] deliveryBoxTypes = new DeliveryBoxType[] {
        DeliveryBoxType.Cardboard,
    };

    public Request MakeRequest(System.Random random, float levelTimeMultiplier) 
    {
        Request request = new Request(this, this.patienceTime * levelTimeMultiplier);
        
        PieceSkin globalSkin = this.skins[random.Next(this.skins.Length)];

        foreach (RequestPiece part in this.compulsoryParts)
        {
            this._AddPiece(request, part, this._GetPieceSkin(random, globalSkin));
        }

        foreach (RequestPiecePool pool in this.optionalPartsPools)
        {
            RequestPiece piece = pool.pieces[random.Next(pool.pieces.Length)];
            if (piece.data != null)
            {
                this._AddPiece(request, piece, this._GetPieceSkin(random, globalSkin));
            }
        }

        request.deliveryBoxType = this.deliveryBoxTypes[random.Next(this.deliveryBoxTypes.Length)];
        return request;
    }

    private PieceSkin _GetPieceSkin(System.Random random, PieceSkin globalSkin)
    {
        return (this.perPartSkin) 
            ? this.skins[random.Next(this.skins.Length)]
            : globalSkin;
    }

    private void _AddPiece(Request request, RequestPiece requestPiece, PieceSkin skin)
    {
        request.AddPiece(new RequestPiece(requestPiece.data, requestPiece.direction, requestPiece.data.skinable ? skin : null));
    }

}