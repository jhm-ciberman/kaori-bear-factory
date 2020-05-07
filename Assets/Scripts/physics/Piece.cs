﻿using Hellmade.Sound;
using UnityEngine;

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

            if (go.tag == "Dispawner")
            {
                this.piece.Dispawn();
                return;
            }

            if (go.tag == "Belt")
            {
                Belt belt = go.GetComponent<Belt>();
                this.piece.MoveByBelt(belt);
                return;
            }
        }

        void OnCollisionEnter(Collision other)
        {
            GameObject go = other.gameObject;
            if (go.tag == "Dispawner") return;

            this.piece.CollisionWithOther();
        }
    }

    public static float globalScale = 0.15f;

    public event System.Action<Piece> onAttached;

    public event System.Action<Piece> onDragStart;

    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private Collider _dragHitbox = null;
    [SerializeField] private MeshRenderer _model = null;
    [SerializeField] private PieceSkinRenderingData _renderingData = null;
    [SerializeField] public PieceData pieceData = null;
    [SerializeField] public AudioClip impactSound = null;
    [SerializeField] public AudioClip snapSound = null;


    private bool _isDraggable = true;
    private bool _isDragged = false;
    private bool _isAttached = false;
    private Transform _transform;
    private PieceSkin _skin = null;

    private Vector3 _localCenter;

    public void Awake()
    {
        if (this.pieceData.skinable)
        {
            this._skin = new PieceSkin(this._renderingData);
        }
    }

    protected void Start()
    {
        this._transform = this._rigidbody.transform;
        this._rigidbody.gameObject.AddComponent<CollisionDetection>().piece = this;
        this._dragHitbox.gameObject.AddComponent<Hitbox>().piece = this;
        this._model.gameObject.layer = 8; // layer 8

        this._localCenter = this._rigidbody.centerOfMass;
        this._model.transform.localPosition -= this._localCenter;
        this.transform.localScale = Piece.globalScale * Vector3.one;

        this._UpdateRigidBodyState();
    }

    public Vector3 localCenter => this._localCenter;

    public void MoveByBelt(Belt belt)
    {
        if (this._isDragged) return;

        this._rigidbody.MovePosition(this._transform.position + belt.velocity * Time.deltaTime);
    }

    public void UpdatePosition(Vector3 pos)
    {
        this._rigidbody.MovePosition(pos);
    }

    public void UpdateRotation(Vector3 pos, Quaternion rotation)
    {
        var origin = this._rigidbody.worldCenterOfMass - this._rigidbody.position;
        this._rigidbody.MovePosition(pos - origin);
        this._rigidbody.MoveRotation(rotation);
    }

    private float _GetScale(PieceDirection dir)
    {
        if (dir == PieceDirection.Left) return 1f;
        if (dir == PieceDirection.Right) return -1f;
        return 1f;
    }

    public virtual void Dispawn()
    {
        Object.Destroy(this.gameObject);
    }

    public bool draggable
    {
        get => this._isDraggable;
        set
        {
            this._isDraggable = value;
            this._rigidbody.detectCollisions = value;

            if (this._dragHitbox.gameObject != this._rigidbody.gameObject)
            {
                this._dragHitbox.gameObject.SetActive(value);
            }
        }
    }

    public void AttachToCraftable(CraftablePiece craftable, PieceDirection dir)
    {
        Transform t = this.modelTransform;
        t.parent = craftable.modelTransform;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = new Vector3(this._GetScale(dir), 1f, 1f);
        this._rigidbody.isKinematic = true;
        this.transform.parent = craftable.transform;
        
        this._AfterAtttach();
    }

    public void AttachToSpot(Transform attachSpot)
    {
        Transform t = this.modelTransform;
        t.parent = attachSpot;
        Debug.Log(this.localCenter);
        t.localPosition -= this.localCenter;
        t.localRotation = Quaternion.identity;
        this._AfterAtttach();
    }

    protected void _AfterAtttach()
    {
        this._isAttached = true;
        this._UpdateRigidBodyState();

        this.onAttached?.Invoke(this);

        if (this.snapSound)
        {
            EazySoundManager.PlaySound(this.snapSound);
        }
    }

    private void Update()
    {
        this._skin?.UpdateMaterial(this._model);
    }

    public void Deattach()
    {
        this._isAttached = false;
        this._UpdateRigidBodyState();
        this.modelTransform.parent = this.transform;
        this.modelTransform.localPosition += this._rigidbody.centerOfMass;
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

    public PieceSkin skin => this._skin;

    public Transform modelTransform => this._model.transform;

    public Vector3 rigidbodyPosition => this._rigidbody.position;
    public Vector3 rigidbodyWorldCenter => this._rigidbody.worldCenterOfMass;
    public Quaternion rigidbodyRotation => this._rigidbody.rotation;

    public void CollisionWithOther()
    {
        if (this.impactSound)
        {
            EazySoundManager.PlaySound(this.impactSound);
        }
    }
}
