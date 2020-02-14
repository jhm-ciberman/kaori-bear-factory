using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private PlayerInteraction _playerInteraction = null;

    [SerializeField] private PlayerMovement _playerMovement = null;

    public void EnableInput()
    {
        this._playerInteraction.EnableInteraction();
        this._playerMovement.EnableMovement();
    }

    public void DisableInput()
    {
        this._playerInteraction.DisableInteraction();
        this._playerMovement.DisableMovement();
    }
}
