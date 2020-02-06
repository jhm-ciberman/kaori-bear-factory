using UnityEngine;


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

    private float _raycastDistance = 100f;

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
        if (1f - dx < limit)
        {
            this._playerMovement.Move(1);
        }

        if (dx < limit)
        {
            this._playerMovement.Move(-1);
        }
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
            }
        }

        this._UpdateCamera();
    }

    public void StopInteraction()
    {
        this._drag.EndDrag();
    }
}