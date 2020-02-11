using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField]
    private UIManager _uiManager = null;

    [SerializeField]
    private PlayerInteraction _playerInteraction = null;

    [SerializeField]
    private PlayerMovement _playerMovement = null;

    void Update()
    {
        this._playerInteraction.SetCursorPosition(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            this._playerInteraction.StartInteraction();
        }

        if (Input.GetMouseButtonUp(0))
        {
            this._playerInteraction.StopInteraction();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            this._playerMovement.SetNormalizedValue(1);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            this._playerMovement.SetNormalizedValue(-1);
        }
    }

}
