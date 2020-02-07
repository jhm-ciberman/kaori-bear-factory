using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = null;

    public LayerMask tableLayer;

    [SerializeField]
    private DragState _drag;

    [HideInInspector]
    private Vector2 _cursorScreenPos;

    [SerializeField]
    private PlayerMovement _playerMovement;

    [SerializeField]
    private ScrollRect _scrollView;

    [SerializeField]
    private float _moveSpeed = 1f;

    private float _raycastDistance = 100f;

    void Start()
    {
        this._playerMovement.SetNormalizedValue(this._scrollView.horizontalNormalizedPosition);
    }

    public void SetCursorPosition(Vector2 pos)
    {
        this._cursorScreenPos = pos;
    }

    public void FixedUpdate()
    {
        if (this._drag.isDragging)
        {
            RaycastHit hit;
            Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);
            if (Physics.Raycast(ray, out hit, this._raycastDistance, this.tableLayer))
            {
                this._drag.targetPos = hit.point;
            }

            this._drag.UpdateDrag(Time.deltaTime);
            this._UpdateCamera();
        }
    }

    private void _UpdateCamera()
    {
        float dx = this._cursorScreenPos.x / Screen.width;
        float limit = 0.10f;

        if (1f - dx < limit) // Move right
        {
            float force = 1f - ((1f - dx) / limit);
            this._viewPos += force * this._moveSpeed * Time.deltaTime;

            if (this._viewPos > this._playerMovement.stations.Length - 2) 
                this._viewPos = this._playerMovement.stations.Length - 2;
                
            this._playerMovement.SetNormalizedValue(this._viewPos);
        }

        if (dx < limit) // Move left
        {
            float force = 1f - (dx / limit);
            this._viewPos -= force * this._moveSpeed * Time.deltaTime;

            if (this._viewPos < 0) 
                this._viewPos = 0;

            this._playerMovement.SetNormalizedValue(this._viewPos);
        }
    }

    private float _viewPos
    {
        get => this._scrollView.horizontalNormalizedPosition;
        set => this._scrollView.horizontalNormalizedPosition = value;
    }

    public void StartInteraction()
    {
        RaycastHit hit;
        Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);

        if (Physics.Raycast(ray, out hit, this._raycastDistance, 1 << Piece.PieceHitbox.layer))
        {
            Piece.PieceHitbox hitbox = hit.collider.GetComponent<Piece.PieceHitbox>();

            if (hitbox && hitbox.piece)
            {
                this._drag.EndDrag();
                this._drag.StartDrag(hitbox.piece, hit.point);
                this._scrollView.gameObject.SetActive(false);
            }
        }

        this._UpdateCamera();
    }

    public void StopInteraction()
    {
        this._scrollView.gameObject.SetActive(true);
        this._drag.EndDrag();
    }
}