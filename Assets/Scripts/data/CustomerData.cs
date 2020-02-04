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

    private System.Random _random = new System.Random();

    public Request MakeRequest(System.Random random) 
    {
        Request request = new Request(this);
        
        PieceSkin globalSkin = this._GetRandomSkin(random);

        foreach (var part in this.compulsoryParts)
        {
            this._AddPiece(request, part, globalSkin);
        }

        foreach (RequestPiecePool pool in this.optionalPartsPools)
        {
            RequestPiece piece = pool.pieces[this._random.Next(pool.pieces.Length)];
            if (piece.data != null)
            {
                if (this.perPartSkin) 
                {
                    this._AddPiece(request, piece, this._GetRandomSkin(this._random));
                }
                else
                {
                    this._AddPiece(request, piece, globalSkin);
                }
            }
        }

        request.maximumTime = this.patienceTime;
        request.deliveryBoxType = this.deliveryBoxTypes[random.Next(this.deliveryBoxTypes.Length)];
        return request;
    }

    private PieceSkin _GetRandomSkin(System.Random random)
    {
        return this.skins[random.Next(this.skins.Length)];
    }

    private void _AddPiece(Request request, RequestPiece requestPiece, PieceSkin skin)
    {
        request.AddPiece(new RequestPiece(requestPiece.data, requestPiece.direction, requestPiece.data.skineable ? skin : null));
    }

}