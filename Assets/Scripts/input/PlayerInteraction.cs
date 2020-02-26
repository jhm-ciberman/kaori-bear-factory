using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private Camera _camera = null;
    
    [SerializeField] public LayerMask tableLayer;

    [SerializeField] public LayerMask interactionLayer;

    [SerializeField] public LayerMask dragAreaLayer;
    
    [SerializeField] private DragState _drag = new DragState();
    
    [SerializeField] private PlayerMovement _playerMovement = null;

    [Range(0f, 1f)] 
    [SerializeField]public float screenDragMargin = 0.10f;

    private Vector2 _cursorScreenPos = new Vector2();
    
    private float _raycastDistance = 100f;

    private bool _canInteract = true;

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

    void Update()
    {
        if (! this._canInteract) return;
        
        this._cursorScreenPos = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            this.StartInteraction();
        }

        if (Input.GetMouseButtonUp(0))
        {
            this.StopInteraction();
        }
    }

    public void FixedUpdate()
    {
        if (this._drag.isDragging)
        {
            this._drag.targetPos = this._GetTableDragTarget(this._drag.targetPos);

            this._drag.UpdateDrag(Time.deltaTime);
            this._UpdateCamera();
        }
    }

    private Vector3 _GetTableDragTarget(Vector3 defaultTargetPos)
    {
        RaycastHit hit;
        Ray ray = this._camera.ScreenPointToRay(this._cursorScreenPos);
        if (Physics.Raycast(ray, out hit, this._raycastDistance, this.tableLayer))
        {
            return hit.point;
        }

        return defaultTargetPos;
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
        if (Physics.Raycast(ray, out hit, this._raycastDistance, this.interactionLayer))
        {
            Piece piece = hit.collider.GetComponent<Piece.Hitbox>()?.piece;

            Vector3 elevationUpVector = this.GetElevationUpVector(hit.point);
            
            if (piece != null)
            {
                this._drag.EndDrag();
                Vector3 targetPos = this._GetTableDragTarget(hit.point);
                this._drag.StartDrag(piece, targetPos, elevationUpVector);
                this._playerMovement.DisableMovement();
                return;
            }

            MachineButton button = hit.collider.GetComponent<MachineButton>();
            button?.Interact();
        }
    }

    private Vector3 GetElevationUpVector(Vector3 pos)
    {
        var colliders = Physics.OverlapSphere(pos, 0.1f, this.dragAreaLayer);
        foreach (var collider in colliders)
        {
            DragArea dragArea = collider.GetComponent<DragArea>();
            if (dragArea != null)
            {
                return dragArea.elevationDirection;
            }
        }

        return Vector3.up;
    }

    public void StopInteraction()
    {
        if (! this._canInteract) return;
        this._playerMovement.enabled = true;
        this._drag.EndDrag();
        this._playerMovement.EnableMovement();
    }
}