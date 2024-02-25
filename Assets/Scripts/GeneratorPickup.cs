using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GeneratorPickup : Interactable
{
    public UnityEvent onPickup;

    public override void Interact(PlayerInteractables interacter)
    {
        onPickup?.Invoke();
        gameObject.SetActive(false);
        Destroy(this.gameObject,0.2f);
    }

}
