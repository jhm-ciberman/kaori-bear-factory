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
    }

    public void StopInteraction()
    {
        this._drag.EndDrag();
    }
}