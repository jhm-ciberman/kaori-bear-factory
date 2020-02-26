using UnityEngine;

[CreateAssetMenu(fileName = "SkinData", menuName = "Game/SkinData", order = 1)]
public class SkinData : ScriptableObject
{
    public Color uiIconColor = Color.white;

    public Color materialColor = Color.white;

    public Color lightColor = Color.white;

    public Texture2D albedo = null;
}