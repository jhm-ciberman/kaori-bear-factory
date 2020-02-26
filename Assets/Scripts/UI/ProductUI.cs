using UnityEngine;
using UnityEngine.UI;

public class ProductUI : MonoBehaviour
{
    public RectTransform baseLayer;

    public Image productBoxImage;

    public Sprite spriteCardboardBox;

    public Sprite spriteGiftBox;

    void Start()
    {
        this.baseLayer.gameObject.SetActive(false);
    }

    public void SetRequest(Request request)
    {
        foreach (RequestPiece piece in request.GetPieces())
        {
            RectTransform transform = Object.Instantiate(this.baseLayer, this.baseLayer.position, Quaternion.identity, this.baseLayer.parent);
            Image image = transform.GetComponent<Image>();
            image.sprite = this._GetSprite(piece);
            image.color = piece.skin ? piece.skin.uiIconColor : Color.white;

            transform.gameObject.SetActive(true);
        }

        if (request.deliveryBoxType == DeliveryBoxType.Cardboard)
        {
            this.productBoxImage.sprite = this.spriteCardboardBox;
        }
        else
        {
            this.productBoxImage.sprite = this.spriteGiftBox;
        }
    }

    private Sprite _GetSprite(RequestPiece piece)
    {
        return (piece.direction == PieceDirection.Left)
            ? piece.data.uiLayerSpriteLeft
            : (piece.direction == PieceDirection.Right)
                ? piece.data.uiLayerSpriteRight
                : piece.data.uiLayerSpriteNone;
    }
}