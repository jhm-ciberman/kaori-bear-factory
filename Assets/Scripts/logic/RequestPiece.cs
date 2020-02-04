using UnityEngine;

[System.Serializable]
public class RequestPiece// : IEquatable
{
    public PieceData data;

    public PieceDirection direction;

    [HideInInspector]
    public PieceSkin skin = null;

    public RequestPiece(PieceData data, PieceDirection direction, PieceSkin skin)
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
        //int hashSkin = (this.skin == null) ? 0 : this.skin.GetHashCode();
        return (this.data.type + "_" + this.direction).GetHashCode();
    }
}