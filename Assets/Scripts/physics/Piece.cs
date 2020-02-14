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

    [SerializeField] private Rigidbody _rigidbody = null;
    [SerializeField] private Collider _dragHitbox = null;
    [SerializeField] private MeshRenderer _model = null;
    [SerializeField] private Transform _center = null;

    [HideInInspector] public PieceData pieceData;

    private bool _isDragged = false;
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

        this._ForceDragStatus(false);
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
        set =>this._rigidbody.detectCollisions = value;
    }

    public void Attach(CraftablePiece craftable, PieceDirection dir)
    {
        this.Attach(craftable.modelTransform, dir);
    }

    public void Attach(Transform attachSpot, PieceDirection dir = PieceDirection.None)
    {
        Transform t = this.modelTransform;
        t.parent = attachSpot;
        t.localPosition = Vector3.zero;
        t.localRotation = Quaternion.identity;
        t.localScale = new Vector3(this._GetScale(dir), 1f, 1f);

        this._SetAttachState(true);
        this.onAttached?.Invoke(this);
    }

    public void Deattach()
    {
        this._SetAttachState(false);
        this.modelTransform.parent = null;
        this.modelTransform.localPosition += this._centerPos;
    }

    private void _ForceDragStatus(bool dragStatus)
    {
        this._isDragged = dragStatus;
        this._rigidbody.useGravity = !dragStatus;
        this._rigidbody.constraints = dragStatus ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.None;
    }

    private void _SetAttachState(bool attached)
    {
        this._rigidbody.useGravity       = !attached;
        this._rigidbody.isKinematic      = attached;
        this._rigidbody.constraints      = RigidbodyConstraints.None;
        this._isDragged = false;
    }

    public bool isDragged
    {
        get => this._isDragged;
        set 
        {
            if (this._isDragged == value) return;
            this._ForceDragStatus(value);
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
