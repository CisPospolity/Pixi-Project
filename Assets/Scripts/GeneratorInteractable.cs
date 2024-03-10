using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorInteractable : Interactable
{
    [SerializeField]
    private GameObject generator;
    [SerializeField]
    private GameObject trigger;
    public override void Interact(PlayerInteractables interacter)
    {
        generator.SetActive(true);
        trigger.SetActive(true);
        interactable = false;
    }
}
