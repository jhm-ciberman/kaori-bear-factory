using UnityEngine;

public class Piece : MonoBehaviour
{
    public class PieceHitbox : MonoBehaviour
    {

        public static int layer = 8; // Hardcoded

        public Piece piece;

        void Start()
        {
            this.gameObject.layer = PieceHitbox.layer;
        }
    }

    public class PieceCollisionDetection : MonoBehaviour
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
                Object.Destroy(this.gameObject);
            }
        }
    }

    public static float globalScale = 0.15f;

    private bool _isDragged = false;


    [HideInInspector]
    public PieceData pieceData;

    private PieceSkin _skin = null;
    public PieceSkin skin 
    {
        get => this._skin;
        set 
        {
            if (this._skin == value) return;
            this._skin = value;
            
            if (value != null)
            {
                this._model.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", value.albedo); 
            }
        }
    }

    [SerializeField]
    private Rigidbody _rigidbody = null;

    [SerializeField]
    private Collider _dragHitbox = null;

    [SerializeField]
    private Transform _model = null;

    [SerializeField]
    private Transform _center = null;

    private Transform _transform;
    private Vector3 _centerPos;

    public void Start()
    {
        this._transform = this._rigidbody.transform;
        this._rigidbody.gameObject.AddComponent<PieceCollisionDetection>().piece = this;
        this._dragHitbox.gameObject.AddComponent<PieceHitbox>().piece = this;

        this._centerPos = this._center.localPosition;
        this._model.localPosition -= this._centerPos;
        this.transform.localScale = Piece.globalScale * Vector3.one;

        this.SetDragStatus(false);
    }

    public Vector3 GetDragOffset(Vector3 dragPosition)
    {
        return dragPosition - this._transform.position;
    }

    public void SetDragStatus(bool isDragged)
    {
        this._isDragged = isDragged;
        this._rigidbody.useGravity = !isDragged;
        this._rigidbody.constraints = isDragged ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.None;
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

    public void Attach(Craftable craftable, PieceDirection dir)
    {
        this._model.parent = craftable.piece._model.transform;
        this._model.localPosition = Vector3.zero;
        this._model.localRotation = Quaternion.identity;
        this._model.localScale = new Vector3(this._GetScale(dir), 1f, 1f);

        this._SetAttachState(true);
    }

    public void Deattach()
    {
        this._SetAttachState(false);
        this._model.parent = null;
        this._model.localPosition += this._centerPos;

    }

    private void _SetAttachState(bool attached)
    {
        this._rigidbody.useGravity       = !attached;
        this._rigidbody.detectCollisions = !attached;
        this._rigidbody.isKinematic      = attached;
        this._isDragged = false;
    }
}
