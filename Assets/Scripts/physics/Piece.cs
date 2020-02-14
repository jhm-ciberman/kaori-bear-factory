﻿using UnityEngine;

[DisallowMultipleComponent]
public class Piece : MonoBehaviour
{
    public class Hitbox : MonoBehaviour
    {
        public static int layer = 8; // Hardcoded

        public Piece piece;

        void Start()
        {
            this.gameObject.layer = Hitbox.layer;
        }
    }

    public class CollisionDetection : MonoBehaviour
    {
        public Piece piece;

        void OnCollisionStay(Collision other)
        {
            GameObject go = other.gameObject;
            if (go.tag == "Belt")
            {
                Belt belt = go.GetComponent<Belt>();
                this.piece.MoveByBelt(belt);
            }

            if (go.tag == "Dispawner")
            {
                this.piece.Dispawn();
            }
        }
    }

    public static float globalScale = 0.15f;

    public event System.Action<Piece> onAttached;

    public event System.Action<Piece> onDragStart;

    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private Collider _dragHitbox = null;
    [SerializeField] private MeshRenderer _model = null;
    [SerializeField] private Transform _center = null;

    [HideInInspector] public PieceData pieceData;

    private bool _isDragged = false;
    private bool _isAttached = false;
    private PieceSkin _skin = null;
    private Transform _transform;
    private Vector3 _centerPos;

    public void Start()
    {
        this._transform = this._rigidbody.transform;
        this._rigidbody.gameObject.AddComponent<CollisionDetection>().piece = this;
        this._dragHitbox.gameObject.AddComponent<Hitbox>().piece = this;

        this._centerPos = this._center.localPosition;
        this._model.transform.localPosition -= this._centerPos;
        this.transform.localScale = Piece.globalScale * Vector3.one;

        this._UpdateRigidBodyState();
    }

    private void _SetMaterialBySkin(PieceSkin skin)
    {
        if (skin == null) return;
        this._model.material.SetTexture("_BaseMap", skin.albedo); 
    }

    public Vector3 GetDragOffset(Vector3 dragPosition)
    {
        return dragPosition - this._transform.position;
    }

    public void MoveByBelt(Belt belt)
    {
        if (this._isDragged) return;

        this._rigidbody.MovePosition(this._transform.position + belt.velocity * Time.deltaTime);
    }

    public void UpdatePosition(Vector3 pos)
    {
        this._rigidbody.MovePosition(pos);
    }

    private float _GetScale(PieceDirection dir)
    {
        if (dir == PieceDirection.Left) return -1f;
        if (dir == PieceDirection.Right) return 1f;
        return 1f;
    }

    public void Dispawn()
    {
        Object.Destroy(this.gameObject);
    }

    public bool draggable
    {
        get => this._rigidbody.detectCollisions;
        set => this._rigidbody.detectCollisions = value;
    }

    public void Attach(CraftablePiece craftable, PieceDirection dir)
    {
        this.Attach(craftable.modelTransform, dir);
        this._rigidbody.isKinematic = this._isAttached;
    }

    public void Attach(Transform attachSpot, PieceDirection dir = PieceDirection.None)
    {
        Transform t = this.modelTransform;
        t.parent = attachSpot;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = new Vector3(this._GetScale(dir), 1f, 1f);

        this._isAttached = true;
        this._UpdateRigidBodyState();

        this.onAttached?.Invoke(this);
    }

    public void Deattach()
    {
        this._isAttached = false;
        this._UpdateRigidBodyState();
        this.modelTransform.parent = this.transform;
        this.modelTransform.localPosition += this._centerPos;
    }

    private void _UpdateRigidBodyState()
    {
        bool isFree = (!this._isDragged && !this._isAttached);
        this._rigidbody.useGravity = isFree;
        this._rigidbody.constraints = isFree ?  RigidbodyConstraints.None : RigidbodyConstraints.FreezeRotation;
        
    }

    public bool isAttached
    {
        get => this._isAttached;
    }

    public bool isDragged
    {
        get => this._isDragged;
        set 
        {
            if (this._isDragged == value) return;
            this._isDragged = value;
            this._UpdateRigidBodyState();
            if (value) this.onDragStart?.Invoke(this);
        }
    }

    public PieceSkin skin 
    {
        get => this._skin;
        set 
        {
            if (this._skin == value) return;
            this._skin = value;
            this._SetMaterialBySkin(value);
        }
    }

    public Transform modelTransform
    {
        get => this._model.transform;
    }
}
