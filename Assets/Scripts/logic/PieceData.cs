using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Game/PieceData", order = 1)]
public class PieceData : ScriptableObject
{
    public GameObject piecePrefab;

    public PieceType type;

    public bool skinable = false;

    public Sprite uiLayerSpriteNone;
    public Sprite uiLayerSpriteLeft;
    public Sprite uiLayerSpriteRight;
}