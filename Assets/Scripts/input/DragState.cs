
using UnityEngine;

[System.Serializable]
public class DragState
{
    [SerializeField] public float _elevation = 1f;

    [SerializeField] public float _animationDuration = 0.15f;

    private Vector3 _offset;
    private Piece _piece;
    private float _realElevation = 0f;
    private float _animationTime = 0f;
    private Vector3 _target;

    public void StartDrag(Piece piece, Vector3 pos)
    {
        this._piece = piece;
        this._target = pos;
        this._offset = this._piece.GetDragOffset(pos);
        this._realElevation = 0f;
        this._animationTime = 0f;
        this._piece.isDragged = true;
        this._piece.onAttached += this._OnPieceAttached;
    }

    private void _OnPieceAttached(Piece piece)
    {
        this.EndDrag();
    }

    public bool isDragging
    {
        get => (this._piece != null);
    }

    public Vector3 targetPos
    {
        get => this._target;
        set => this._target = value;
    }

    public void UpdateDrag(float deltatime)
    {
        if (! this.isDragging) return;
        
        this._animationTime += deltatime;

        if (this._animationTime > this._animationDuration)
        {
            this._animationTime = this._animationDuration;
        }
        float p = this._animationTime / this._animationDuration;
        p = -p * (p - 2); // ease quad out

        this._realElevation = this._elevation * p;
        this._piece.UpdatePosition(this._target - this._offset + this._realElevation * Vector3.up);
    }

    public void EndDrag()
    {
        if (! this.isDragging) return;
        this._piece.isDragged = false;
        this._piece.onAttached -= this._OnPieceAttached;
        this._piece = null;

        this.UpdateDrag(0f);
    }
}