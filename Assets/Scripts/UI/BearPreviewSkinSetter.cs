using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class BearPreviewSkinSetter : MonoBehaviour
{
    public SkinData skin;

    public PieceSkinRenderingData pieceSkinRenderingData;

    private void SetBearColor()
    {
        var pieceSkin = new PieceSkin(this.pieceSkinRenderingData);
        pieceSkin.data = skin;
        
        foreach (var renderer in this.GetComponentsInChildren<MeshRenderer>())
        {
            pieceSkin.UpdateMaterial(renderer);
        }
    }

    public void Awake()
    {
        this.SetBearColor();
    }
}