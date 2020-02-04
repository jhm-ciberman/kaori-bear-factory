using UnityEngine;

public class PieceCollisionDetection : MonoBehaviour
{
    public Piece piece;

    void Start()
    {
        this.gameObject.layer = 8; // hardcoded!
    }

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