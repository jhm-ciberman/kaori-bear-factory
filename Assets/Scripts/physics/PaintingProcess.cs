using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingProcess
{
    private List<Piece> _piecesToPaint = new List<Piece>();

    private float _paintedPieces = 0f;
    
    private SkinData _skin = null;

    private float _timePerPiece;

    public PaintingProcess(SkinData skin, Piece piece, float timePerPiece)
    {
        this._paintedPieces = 0f;
        this._skin = skin;
        this._timePerPiece = timePerPiece;

        this._AddPiecesToPaint(this._piecesToPaint, piece);
    }

    public int count
    {
        get => this._piecesToPaint.Count;
    }

    public float progress
    {
        get => this._paintedPieces / this._piecesToPaint.Count;
    }

    public void Update(float deltaTime)
    {
        this._paintedPieces += deltaTime / this._timePerPiece;
        int paintedPieces = Mathf.FloorToInt(this._paintedPieces);
        int unpaintedPieces = Mathf.CeilToInt(this._paintedPieces);

        for (int i = 0; i < this._piecesToPaint.Count; i++)
        {
            Piece piece = this._piecesToPaint[i];

            if (i < paintedPieces)
            {
                
            }
        }
    }

    void _AddPiecesToPaint(List<Piece> list, Piece piece)
    {
        if (piece.pieceData.skinable)
        {
            list.Add(piece);
        }

        if (piece is CraftablePiece)
        {
            CraftablePiece craftable = piece as CraftablePiece;

            foreach (var subpiece in craftable.attachedPieces)
            {
                this._AddPiecesToPaint(list, subpiece);
            }
        }
    }

}