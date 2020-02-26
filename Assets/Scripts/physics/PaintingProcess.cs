using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingProcess
{
    private List<Piece> _piecesToPaint = new List<Piece>();

    private float _paintedPieces = 0f;
    
    private SkinData _skin = null;

    private float _timePerPiece;

    public System.Action onFinished;

    public PaintingProcess(SkinData skin, Piece piece, float timePerPiece)
    {
        this._paintedPieces = 0f;
        this._skin = skin;
        this._timePerPiece = timePerPiece;

        this._AddPiecesToPaint(this._piecesToPaint, piece, skin);
    }

    public int count => this._piecesToPaint.Count;

    public float paintedPieces => this._paintedPieces;

    public float progress => this._paintedPieces / this._piecesToPaint.Count;

    public bool hasFinished => (this.progress == 1f);

    public void Update(float deltaTime)
    {
        if (this._paintedPieces < this._piecesToPaint.Count)
        {
            this._paintedPieces += deltaTime / this._timePerPiece;

            if (this._paintedPieces >= this._piecesToPaint.Count)
            {
                this._paintedPieces = this._piecesToPaint.Count;
                this._Finish();
                return;
            }
            
            this._SetPiecesProgres(this.progress);
        }
    }

    private void _SetPiecesProgres(float progress)
    {
        foreach (Piece piece in this._piecesToPaint)
        {
            piece.skin.secondaryData = this._skin;
            piece.skin.transition = progress;
        }
    }

    public void CancelPainting()
    {
        foreach (Piece piece in this._piecesToPaint)
        {
            piece.skin.secondaryData = null;
            piece.skin.transition = 0f;
        }
    }

    private void _Finish()
    {
        foreach (Piece piece in this._piecesToPaint)
        {
            piece.skin.data = this._skin;
            piece.skin.secondaryData = null;
            piece.skin.transition = 0f;
        }

        this.onFinished?.Invoke();
    }

    void _AddPiecesToPaint(List<Piece> list, Piece piece, SkinData targetSkin)
    {
        if (piece.pieceData.skinable && piece.skin.data != targetSkin)
        {
            list.Add(piece);
        }

        if (piece is CraftablePiece)
        {
            CraftablePiece craftable = piece as CraftablePiece;

            foreach (var subpiece in craftable.attachedPieces)
            {
                this._AddPiecesToPaint(list, subpiece, targetSkin);
            }
        }
    }

}