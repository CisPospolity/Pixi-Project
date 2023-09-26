using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputSystem : MonoBehaviour, PlayerControls.IGameplayActions
{
    private PlayerControls playerControls;
    private PlayerInput playerInput;
    [SerializeField]
    private bool isGamepad;

    private void Awake()
    {
        playerControls = new PlayerControls();
        playerInput = GetComponent<PlayerInput>();
        playerControls.Gameplay.SetCallbacks(this);
        playerControls.Enable();
    }
    public event Action<Vector2> onLookRotation;
    public event Action<Vector2> onMovement;
    public event Action onJump;

    public void OnMovement(InputAction.CallbackContext context)
    {
        onMovement?.Invoke(context.ReadValue<Vector2>());
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onJump?.Invoke();
        }
    }

    public void OnLookRotation(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onLookRotation?.Invoke(context.ReadValue<Vector2>());
        }
    }

    public void OnDeviceChange(PlayerInput pi)
    {
        isGamepad = pi.currentControlScheme.Equals("Gamepad") ? true : false;
    }

    public bool IsGamepad()
    {
        return isGamepad;
    }
}
