using System.Collections.Generic;
using UnityEngine;

public class PaintingMachine : MonoBehaviour
{

    public class InteriorTrigger : MonoBehaviour
    {
        public event System.Action<Piece> onPieceEnter;
        public event System.Action<Piece> onPieceExit;

        public void OnTriggerEnter(Collider other)
        {
            this._OnTrigger(other, this.onPieceEnter);
        }

        public void OnTriggerExit(Collider other)
        {
             this._OnTrigger(other, this.onPieceExit);
        }

        public void Disable()
        {
            this.GetComponent<Collider>().enabled = false;
        }

        public void Enable()
        {
            this.GetComponent<Collider>().enabled = true;
        }

        private void _OnTrigger(Collider other, System.Action<Piece> callback)
        {
            var pcd = other.GetComponent<Piece.CollisionDetection>();
            if (pcd?.piece != null) callback?.Invoke(pcd.piece);
        }
    }

    private enum MachineStatus
    {
        Empty,
        PieceAttached,
        WaitingForPieceExit,
    }

    [SerializeField] private Collider _interiorTrigger = null;

    [SerializeField] private Transform _attachSpot = null;

    [SerializeField] private float _rotationAnimationSpeed = 1f;

    [SerializeField] private float _positionAnimationSpeed = 1f;

    [SerializeField] private float _rotationAnimationAmount = 25f;

    [SerializeField] private float _positionAnimationAmount = 0.25f;

    private Piece _pieceInside = null;

    private Piece _lastPieceExited = null;

    private List<Piece> _piecesToPaint = new List<Piece>();

    private MachineStatus _status = MachineStatus.Empty;

    private float _animationTime = 0f;

    private InteriorTrigger _trigger;

    void Start()
    {
        this._trigger = this._interiorTrigger.gameObject.AddComponent<InteriorTrigger>();
        this._trigger.onPieceEnter += this._OnPieceEnter;
        this._trigger.onPieceExit += this._OnPieceExit;
    }

    void _OnPieceEnter(Piece piece)
    {
        if (this._pieceInside != null) return;
        if (piece == this._lastPieceExited) return;

        Debug.Log("_OnPieceEnter");

        this._piecesToPaint.Clear();
        this._AddPiecesToPaint(this._piecesToPaint, piece);

        foreach (var p in this._piecesToPaint)
        {
            Debug.Log(p);
        }

        this._AttachPiece(piece);
        this._animationTime = 0f;
    }

    private void _RestartPainting()
    {
        this._DeattachPiece();
        this._lastPieceExited = null;
        this._pieceInside = null;
        this._status = MachineStatus.Empty;
    }

    private void _OnPieceExit(Piece piece)
    {
        if (this._pieceInside != piece) return;
        //if (! piece.isDragged) return;
        Debug.Log("_OnPieceExit");
        this._lastPieceExited = piece;
        this._CheckIfPieceExited(piece);
    }

    private void _AttachPiece(Piece piece)
    {
        this._status = MachineStatus.PieceAttached;
        this._pieceInside = piece;
        this._lastPieceExited = null;
        piece.Attach(this._attachSpot);
        piece.onDragStart += this._OnDragStart;
        piece.onAttached += this._OnAttached;
    }

    private void _OnAttached(Piece piece)
    {
        if (piece == this._pieceInside)
        {
            this._DeattachPiece();
        }
        this._CheckIfPieceExited(piece);
    }

    private void _OnDragStart(Piece piece)
    {
        Debug.Log("_OnDragStart");
        if (piece == this._pieceInside)
        {
            this._DeattachPiece();
        }
        this._CheckIfPieceExited(piece);
    }

    private void _DeattachPiece()
    {
        this._status = MachineStatus.WaitingForPieceExit;
        this._pieceInside.onDragStart -= this._OnDragStart;
        this._pieceInside.onAttached -= this._OnAttached;
        this._pieceInside.Deattach();
    }

    private void _CheckIfPieceExited(Piece piece)
    {
        if (this._status == MachineStatus.WaitingForPieceExit && this._lastPieceExited == piece)
        {
            this._status = MachineStatus.Empty;
            this._lastPieceExited = null;
            this._pieceInside = null;
        }
    }

    void Update()
    {
        if (this._status != MachineStatus.PieceAttached) return;
        if (this._pieceInside == null) return;

        this._animationTime += Time.deltaTime;
        
        Transform t = this._pieceInside.modelTransform;
        
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