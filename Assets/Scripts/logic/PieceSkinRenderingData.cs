using UnityEngine;

[CreateAssetMenu(fileName = "PieceSkinRenderingData", menuName = "Game/PieceSkinRenderingData", order = 1)]
public class PieceSkinRenderingData : ScriptableObject
{
    public Material paintingMaterial = null; 

    public Material nonPaintingMaterial = null;

    public SkinData defaultSkin = null;
}