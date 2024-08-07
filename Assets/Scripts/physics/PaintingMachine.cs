using System.Collections.Generic;
using System.Linq;
using Hellmade.Sound;
using UnityEngine;

public class PaintingMachine : MonoBehaviour
{

    public class InteriorTrigger : MonoBehaviour
    {
        public event System.Action<Piece> onPieceEnter;
        public event System.Action<Piece> onPieceExit;

        public void OnTriggerEnter(Collider other)
        {
            this._OnTrigger(other, this.onPieceEnter);
        }

        public void OnTriggerExit(Collider other)
        {
             this._OnTrigger(other, this.onPieceExit);
        }

        public void Disable()
        {
            this.GetComponent<Collider>().enabled = false;
        }

        public void Enable()
        {
            this.GetComponent<Collider>().enabled = true;
        }

        private void _OnTrigger(Collider other, System.Action<Piece> callback)
        {
            var pcd = other.GetComponent<Piece.CollisionDetection>();
            if (pcd?.piece != null) callback?.Invoke(pcd.piece);
        }
    }

    private enum MachineStatus
    {
        Empty,
        PieceAttached,
        WaitingForPieceExit,
    }

    [SerializeField] private Collider _interiorEnterTrigger = null;

    [SerializeField] private Collider _interiorExitTrigger = null;

    [SerializeField] private Transform _attachSpot = null;

    [SerializeField] private MachineButton[] buttons = null;

    [SerializeField] private float _rotationAnimationSpeed = 1f;

    [SerializeField] private float _positionAnimationSpeed = 1f;

    [SerializeField] private float _rotationAnimationAmount = 25f;

    [SerializeField] private float _positionAnimationAmount = 0.25f;

    [SerializeField] public float timePerPiece = 3f;

    [SerializeField] private Vector3 _rotationDir = Vector3.up;

    [SerializeField] private Light _interiorLight = null;

    [SerializeField] private ParticleSystem _particleSystem = null;

    [SerializeField] private AudioClip _paintingError = null;

    [SerializeField] private AudioClip _paintingFinished = null;

    [SerializeField] private AudioClip _buttonPressedAudio = null;

    [SerializeField] private AudioClip _paintingProcessLoopAudio = null;

    private Piece _pieceInside = null;

    private Piece _lastPieceExited = null;

    private MachineStatus _status = MachineStatus.Empty;

    private float _animationTime = 0f;

    private PaintingProcess _painting = null;

    private Color _pieceInsideColor = Color.gray;

    private int _loopAudioId = -1;

    public System.Action<SkinData> onPaintStart;
    public System.Action<SkinData> onPaintFinish;
    public System.Action onPieceRemoved;
    public System.Action<float, int> onPaintProgress;

    void Start()
    {
        var enterTrigger = this._interiorEnterTrigger.gameObject.AddComponent<InteriorTrigger>();
        var exitTrigger = this._interiorExitTrigger.gameObject.AddComponent<InteriorTrigger>();
        enterTrigger.onPieceEnter += this._OnPieceEnter;
        exitTrigger.onPieceExit += this._OnPieceExit;

        this._SetInteriorLightColor(Color.black);
        this._particleSystem?.Stop();
    }

    public void SetAvailableSkins(SkinData[] availableSkins)
    {
        if (availableSkins.Length < 2) 
        {
            this.gameObject.SetActive(false);
            return;
        }

        foreach (var button in this.buttons)
        {
            button.gameObject.SetActive(availableSkins.Contains(button.skin));
        }
    }

    void _OnPieceEnter(Piece piece)
    {
        if (this._pieceInside != null) return;
        if (piece == this._lastPieceExited) return;

        this._AttachPiece(piece);
        this._animationTime = 0f;
    }

    private void _RestartPainting()
    {
        this._DeattachPiece();
        this._lastPieceExited = null;
        this._pieceInside = null;
        this._status = MachineStatus.Empty;
    }

    private void _OnPieceExit(Piece piece)
    {
        if (this._pieceInside != piece) return;

        this._lastPieceExited = piece;
        this._CheckIfPieceExited(piece);
    }

    private void _AttachPiece(Piece piece)
    {
        this._status = MachineStatus.PieceAttached;
        this._pieceInside = piece;
        this._lastPieceExited = null;
        piece.AttachToSpot(this._attachSpot);
        piece.onDragStart += this._OnDragStart;
        piece.onAttached += this._OnAttached;

        this._SetInteriorLightColor(this._pieceInsideColor);
    }

    private void _OnAttached(Piece piece)
    {
        if (piece == this._pieceInside)
        {
            this._DeattachPiece();
        }
        this._CheckIfPieceExited(piece);
    }

    private void _OnDragStart(Piece piece)
    {
        if (piece == this._pieceInside)
        {
            this._DeattachPiece();
        }
        this._CheckIfPieceExited(piece);
    }

    private void _DeattachPiece()
    {
        if (this._painting != null && ! this._painting.hasFinished)
        {
            this._painting.CancelPainting();
            this._painting = null;
        }

        this._status = MachineStatus.WaitingForPieceExit;
        this._pieceInside.onDragStart -= this._OnDragStart;
        this._pieceInside.onAttached -= this._OnAttached;
        this._pieceInside.Deattach();

        this._particleSystem?.Stop();
        this._SetInteriorLightColor(Color.black);
        this.onPieceRemoved?.Invoke();
    }

    private void _CheckIfPieceExited(Piece piece)
    {
        if (this._status == MachineStatus.WaitingForPieceExit && this._lastPieceExited == piece)
        {
            this._status = MachineStatus.Empty;
            this._lastPieceExited = null;
            this._pieceInside = null;
        }
    }

    void Update()
    {
        if (this._status != MachineStatus.PieceAttached) return;
        if (this._pieceInside == null) return;

        this._animationTime += Time.deltaTime;
        
        Transform t = this._pieceInside.modelTransform;
        
        float rotation = Mathf.Sin(this._animationTime * this._rotationAnimationSpeed) * this._rotationAnimationAmount;
        t.localRotation = Quaternion.AngleAxis(rotation, this._rotationDir);

        float position = Mathf.Sin(this._animationTime * this._positionAnimationSpeed) * this._positionAnimationAmount;
        t.localPosition = this._rotationDir * position - this._pieceInside.localCenter;

        if (this._painting != null)
        {
            this._painting.Update(Time.deltaTime);
            this.onPaintProgress?.Invoke(this._painting.paintedPieces, this._painting.count);
        }
    }

    public void StartPainting(SkinData skin)
    {
        this._particleSystem?.Stop();

        if (this._loopAudioId >= 0)
        {
            EazySoundManager.GetAudio(this._loopAudioId).Stop();
            this._loopAudioId = -1;
        }
        
        if (this._status != MachineStatus.PieceAttached || this._pieceInside == null)
        {
            // Error, no pieces inside. Maybe play a sound?
            if (this._paintingError)
            {
                EazySoundManager.PlaySound(this._paintingError);
            }
            return;
        }

        this._painting = new PaintingProcess(skin, this._pieceInside, this.timePerPiece);
        if (this._painting.count == 0)
        {
            // No pieces to paint. Maybe play another sound? 
            this._painting = null;
            this._SetInteriorLightColor(this._pieceInsideColor);
            if (this._paintingError)
            {
                EazySoundManager.PlaySound(this._paintingError);
            }
            return;
        }

        this._SetInteriorLightColor(skin.lightColor);
        this._particleSystem?.Play();

        this._painting.onFinished = () => {
            this._particleSystem?.Stop();
            this._SetInteriorLightColor(this._pieceInsideColor);
            this.onPaintFinish?.Invoke(skin);

            if (this._loopAudioId >= 0)
            {
               EazySoundManager.GetAudio(this._loopAudioId).Stop();
               this._loopAudioId = -1;
            }
            if (this._paintingFinished)
            {
                EazySoundManager.PlaySound(this._paintingFinished);
            }
        };

        this.onPaintStart?.Invoke(skin);
        if (this._paintingProcessLoopAudio) 
        {
            this._loopAudioId = EazySoundManager.PlaySound(this._paintingProcessLoopAudio, true);
        }

        if (this._buttonPressedAudio)
        {
            EazySoundManager.PlaySound(this._buttonPressedAudio);
        }
    }

    private void _SetInteriorLightColor(Color color)
    {
        if (this._particleSystem)
        {
            var main = this._particleSystem.main;
            main.startColor = color;
        }

        if (this._interiorLight)
        {
            LeanTween.value(this.gameObject, this._interiorLight.color, color, 0.25f)
                .setOnUpdate((value) => this._interiorLight.color = value);
        }
    }
}