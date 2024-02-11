using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : Interactable
{
    [SerializeField]
    private DialogueText dialogueText;
    [SerializeField]
    private DialogueController dialogueController;
    public override void Interact(PlayerInteractables interacter)
    {
        Talk(dialogueText);
    }

    public void Talk(DialogueText dialogueText)
    {
        dialogueController.DisplayNextParagraph(dialogueText);
    }
}
