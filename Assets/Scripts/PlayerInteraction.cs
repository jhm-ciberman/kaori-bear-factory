using UnityEngine;


public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = null;

    public LayerMask piecesLayer;

    public LayerMask tableLayer;

    [HideInInspector]
    private Piece _currentDraggable = null;

    [HideInInspector]
    private Vector2 _cursorScreenPos;

    private float _raycastDistance = 50f;

    public void SetCursorPosition(Vector2 pos)
    {
        this._cursorScreenPos = pos;
    }

    public void Update()
    {
        if (this._currentDraggable != null)
        {
            RaycastHit hit;
            Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);
            if (Physics.Raycast(ray, out hit, this._raycastDistance, this.tableLayer))
            {
                this._currentDraggable.UpdatePosition(hit.point);
            }
        }
    }

    public void StartInteraction()
    {
        RaycastHit hit;
        Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);
        if (Physics.Raycast(ray, out hit, this._raycastDistance, this.piecesLayer))
        {
            var dcd = hit.transform.GetComponent<PieceCollisionDetection>();

            if (dcd && dcd.piece)
            {
                this._currentDraggable = dcd.piece;
                dcd.piece.StartDrag(hit.point);
            }
        }
    }

    public void StopInteraction()
    {
        this._currentDraggable?.EndDrag();
        this._currentDraggable = null;
    }


}