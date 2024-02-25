using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorInteractable : Interactable
{
    [SerializeField]
    private GameObject generator;
    public override void Interact(PlayerInteractables interacter)
    {
        generator.SetActive(true);
        interactable = false;
    }
}
