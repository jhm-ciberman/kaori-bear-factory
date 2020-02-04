using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Game/PieceData", order = 1)]
public class PieceData : ScriptableObject
{
    public GameObject piecePrefab;

    public PieceType type;

    public bool skineable = false;

    public Sprite uiLayerSpriteNone;
    public Sprite uiLayerSpriteLeft;
    public Sprite uiLayerSpriteRight;
}