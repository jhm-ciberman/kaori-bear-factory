using UnityEngine;

[CreateAssetMenu(fileName = "PieceSkin", menuName = "Game/PieceSkin", order = 1)]
public class PieceSkin : ScriptableObject
{
    public Color color;

    public Texture2D albedo;
}