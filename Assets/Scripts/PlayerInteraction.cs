using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private RequestsManager _requestsManager;

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
    [Range(0f, 1f)] public float screenDragMargin = 0.10f;

    private float _raycastDistance = 100f;

    private bool _canInteract = true;

    public void Start()
    {
        this._requestsManager.onLevelComplete += () => this.DisableInteraction();
    }

    public void EnableInteraction()
    {
        this._canInteract = true;
    }

    public void DisableInteraction()
    {
        this._canInteract = false;
        this._drag.EndDrag();
    }

    public void SetCursorPosition(Vector2 pos)
    {
        this._cursorScreenPos = pos;
    }

    public void FixedUpdate()
    {
        if (! this._canInteract) return;

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

        if (1f - dx < this.screenDragMargin) // Move right
        {
            float force = 1f - ((1f - dx) / this.screenDragMargin);
            this._playerMovement.MoveView(force * Time.deltaTime);
            
        }

        if (dx < this.screenDragMargin) // Move left
        {
            float force = 1f - (dx / this.screenDragMargin);
            this._playerMovement.MoveView(-force * Time.deltaTime);
        }
    }



    public void StartInteraction()
    {
        if (! this._canInteract) return;

        RaycastHit hit;
        Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);

        if (Physics.Raycast(ray, out hit, this._raycastDistance, 1 << Piece.PieceHitbox.layer))
        {
            Piece.PieceHitbox hitbox = hit.collider.GetComponent<Piece.PieceHitbox>();

            if (hitbox && hitbox.piece)
            {
                this._drag.EndDrag();
                this._drag.StartDrag(hitbox.piece, hit.point);
                this._playerMovement.DisableMovement();
            }
        }
    }

    public void StopInteraction()
    {
        if (! this._canInteract) return;
        this._playerMovement.enabled = true;
        this._drag.EndDrag();
        this._playerMovement.EnableMovement();
    }
}