using System.Collections.Generic;
using UnityEngine;

public class PaintingMachine : MonoBehaviour
{

    public class InteriorTrigger : MonoBehaviour
    {
        public event System.Action<Piece> onPieceEnter;

        public void OnTriggerEnter(Collider other)
        {
            var pcd = other.GetComponent<Piece.CollisionDetection>();

            if (pcd?.piece != null)
            {
                this.onPieceEnter?.Invoke(pcd.piece);
            }
        }
    }

    [SerializeField] private Collider _interiorTrigger = null;

    [SerializeField] private Transform _attachSpot = null;

    [SerializeField] private float _rotationAnimationSpeed = 1f;

    [SerializeField] private float _positionAnimationSpeed = 1f;

    [SerializeField] private float _rotationAnimationAmount = 25f;

    [SerializeField] private float _positionAnimationAmount = 0.25f;

    private Piece _attachedPiece = null;

    private List<Piece> _piecesToPaint = new List<Piece>();

    private float _animationTime = 0f;

    void Start()
    {
        var trigger = this._interiorTrigger.gameObject.AddComponent<InteriorTrigger>();
        trigger.onPieceEnter += this._OnPieceEnter;
    }

    void _OnPieceEnter(Piece piece)
    {
        if (this._attachedPiece != null) return;

        this._attachedPiece = piece;

        this._piecesToPaint.Clear();
        this._AddPiecesToPaint(this._piecesToPaint, piece);

        foreach (var p in this._piecesToPaint)
        {
            Debug.Log(p);
        }

        piece.Attach(this._attachSpot);
        piece.draggable = true;

        this._animationTime = 0f;
    }

    void Update()
    {
        if (this._attachedPiece == null) return;

        this._animationTime += Time.deltaTime;
        
        Transform t = this._attachedPiece.modelTransform;
        
        float rotation = Mathf.Sin(this._animationTime * this._rotationAnimationSpeed) * this._rotationAnimationAmount;
        t.localRotation = Quaternion.AngleAxis(rotation, Vector3.up);

        float position = Mathf.Sin(this._animationTime * this._positionAnimationSpeed) * this._positionAnimationAmount;
        t.localPosition = Vector3.up * position;
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