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
    public event Action onLightAttack;
    public event Action onHeavyAttack;
    public event Action onJump;
    public event Action onDash;
    public event Action onQuickSkill;
    public event Action onStrongSkill;


    public event Action onInteract;

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

    public void OnLightAttack(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onLightAttack?.Invoke();
        }
    }

    public void OnHeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onHeavyAttack?.Invoke();
        }
    }

    public void OnDash(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onDash?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
       if(context.performed)
        {
            onInteract?.Invoke();
        }
    }

    public void OnQuickSkill(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onQuickSkill?.Invoke();
        }
    }

    public void OnStrongSkill(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            onStrongSkill?.Invoke();
        }
    }

    public void DisableInput()
    {
        playerControls.Disable();
    }

    public void EnableInput()
    {
        playerControls.Enable();
    }
}
