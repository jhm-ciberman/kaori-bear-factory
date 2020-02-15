using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Game/SkinData", order = 1)]
public class SkinData : ScriptableObject
{
    public Color color;

    public Texture2D albedo;
}