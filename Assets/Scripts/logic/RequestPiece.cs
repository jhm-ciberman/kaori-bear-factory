using UnityEngine;

[System.Serializable]
public class RequestPiece// : IEquatable
{
    public PieceData data;

    public PieceDirection direction;

    [HideInInspector]
    public SkinData skin = null;

    public RequestPiece(PieceData data, PieceDirection direction, SkinData skin)
    {
        this.data = data;
        this.direction = direction;
        this.skin = skin;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        RequestPiece otherPiece = (RequestPiece) obj;
        return otherPiece.data.type == this.data.type && otherPiece.direction == this.direction && this.skin == otherPiece.skin;
    }
    
    public override int GetHashCode()
    {
        return (this.data.type + "_" + this.direction).GetHashCode();
    }

    public Color GetColor()
    {
        return this.data.skinable ? this.skin.uiIconColor : Color.white;
    }

    public Sprite GetSprite()
    {
        return (this.direction == PieceDirection.Left)
            ? this.data.uiLayerSpriteLeft
            : (this.direction == PieceDirection.Right)
                ? this.data.uiLayerSpriteRight
                : this.data.uiLayerSpriteNone;
    }
}