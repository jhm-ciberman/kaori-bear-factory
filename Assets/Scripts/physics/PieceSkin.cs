public class PieceSkin
{
    private SkinData _data = null;

    private SkinData _secondaryData = null;

    private float _transition = 0f;

    private bool _dirty = true;

    private PieceSkinRenderingData _renderingData;

    public PieceSkin(PieceSkinRenderingData renderingData)
    {
        this._renderingData = renderingData;
    }

    public SkinData data 
    {
        get => this._data;
        set 
        {
            if (this._data == value) return;
            this._data = value;
            this._dirty = true;
        }
    }

    public SkinData secondaryData
    {
        get => this._secondaryData;
        set 
        {
            if (this._secondaryData == value) return;
            this._secondaryData = value;
            this._dirty = true;
        }
    }

    public float transition
    {
        get => this._transition;
        set
        {
            if (this._transition == value) return;
            this._transition = value;
            this._dirty = true;
        }
    }

    public void UpdateMaterial(UnityEngine.MeshRenderer renderer)
    {
        if (this._data == null) return;
        if (! this._dirty) return;

        if (this._secondaryData != null)
        {   
            renderer.material = this._renderingData.paintingMaterial;
            renderer.material.SetTexture("_BaseMap", this._data.albedo); 
            renderer.material.SetColor("_BaseColor", this._data.materialColor);
            renderer.material.SetTexture("_SecondaryMap", this._secondaryData.albedo); 
            renderer.material.SetColor("_SecondaryColor", this._secondaryData.materialColor);
            renderer.material.SetFloat("_BlendValue", this._transition); 
        }
        else
        {
            renderer.material = this._renderingData.nonPaintingMaterial;
            renderer.material.SetTexture("_BaseMap", this._data.albedo);
            renderer.material.SetColor("_BaseColor", this._data.materialColor);
        }
    }

}
