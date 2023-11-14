using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicLever : Interactable
{
    private Animator animator;
    private bool turnedOn = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public override void Interact(PlayerScript interacter)
    {
        turnedOn = !turnedOn;
        animator.SetBool("turnedOn", turnedOn);
    }
}
