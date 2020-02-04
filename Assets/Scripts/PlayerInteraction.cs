using UnityEngine;


public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera _camera = null;

    public LayerMask piecesLayer;

    public LayerMask tableLayer;

    private Piece _currentDraggable = null;

    private Vector2 _cursorScreenPos;

    public void SetCursorPosition(Vector2 pos)
    {
        this._cursorScreenPos = pos;
    }

    public void Update()
    {
        if (this._currentDraggable != null)
        {
            Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, this.tableLayer))
            {
                this._currentDraggable.UpdatePosition(hit.point);
            }
        }
    }

    public void StartInteraction()
    {
        Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, this.piecesLayer))
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