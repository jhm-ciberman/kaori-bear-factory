using UnityEngine;

public class MachineButton : MonoBehaviour
{
    private Vector3 _startPos;

    public SkinData skin;

    public PaintingMachine paintingMachine;

    void Start()
    {
        this._startPos = this.transform.localPosition;
    }

    public void Interact()
    {
        this.paintingMachine.StartPainting(this.skin);

        this.transform.localPosition = this._startPos;
        LeanTween.moveLocalZ(this.gameObject, -0.15f, 0.15f)
            .setLoopPingPong(1)
            .setOnComplete(() => this.transform.localPosition = this._startPos);
    }
}