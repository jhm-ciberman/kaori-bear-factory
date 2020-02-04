using UnityEngine;

public class Piece : MonoBehaviour
{
    public static float globalScale = 0.15f;

    private bool _isDragged = false;

    private Vector3 _startDragElevation = Vector3.zero;
    private Vector3 _dragOffset         = Vector3.zero;
    private Vector3 _dragDestination    = Vector3.zero;
    private Vector3 _realElevation      = Vector3.zero;
    private Vector3 _elevationVelocity  = Vector3.zero;

    public Rigidbody rigidBody;
    public Vector3 elevation = Vector3.zero;
    public float elevationAnimationTime = 0.15f;

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
                this.model.GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", value.albedo); 
            }
        }
    }

    public Transform model;
    public Transform center;

    private Transform _transform;
    private Vector3 _centerPos;

    public void Start()
    {
        this._transform = this.rigidBody.transform;
        PieceCollisionDetection cd = this.rigidBody.gameObject.AddComponent<PieceCollisionDetection>();
        cd.piece = this;

        this._centerPos = this.center.localPosition;
        this.model.localPosition -= this._centerPos;
        this.transform.localScale = Piece.globalScale * Vector3.one;
    }

    public void StartDrag(Vector3 posStart)
    {
        this._isDragged = true;
        this.rigidBody.useGravity = false;
        this.rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        this._dragDestination = posStart;
        this._dragOffset = posStart - this._transform.position;
    }

    public void EndDrag()
    {
        this._isDragged = false;
        this.rigidBody.useGravity = true;
        this._realElevation = Vector3.zero;
        this.rigidBody.constraints = RigidbodyConstraints.None;
        this._elevationVelocity = Vector3.zero;
    }

    public void MoveByBelt(Belt belt)
    {
        if (this._isDragged) return;

        this.rigidBody.MovePosition(this._transform.position + belt.velocity * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        
        if (this._isDragged)
        {
            this._realElevation = Vector3.SmoothDamp(this._realElevation, this.elevation, ref this._elevationVelocity, this.elevationAnimationTime);

            this.rigidBody.MovePosition(this._dragDestination + this._realElevation - this._dragOffset);
        }
    }

    public void UpdatePosition(Vector3 pos)
    {
        if (this._isDragged)
        {
            this._dragDestination = pos;
        }
    }

    private float _GetScale(PieceDirection dir)
    {
        if (dir == PieceDirection.Left) return -1f;
        if (dir == PieceDirection.Right) return 1f;
        return 1f;
    }

    public void Attach(Craftable craftable, PieceDirection dir)
    {
        this.model.parent = craftable.piece.model.transform;
        this.model.localPosition = Vector3.zero;
        this.model.localRotation = Quaternion.identity;
        this.model.localScale = new Vector3(this._GetScale(dir), 1f, 1f);

        this.rigidBody.detectCollisions = false;

        this.rigidBody.useGravity = false;
        this.rigidBody.isKinematic = true;
        this._isDragged = false;
    }

    public void Deattach()
    {
        this.model.parent = null;
        this.model.localPosition += this._centerPos;
        this.rigidBody.useGravity = true;
        this.rigidBody.isKinematic = false;

        this.rigidBody.detectCollisions = true;
    }
}
