using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : Interactable
{
    [SerializeField]
    private DialogueText dialogueText;
    [SerializeField]
    private DialogueController dialogueController;

    private bool isConversationStarted = false;

    public override void Interact(PlayerInteractables interacter)
    {
        if (!isConversationStarted)
        {
            StartDialogue();
        }
        else
        {
            ContinueDialogue();
        }
    }

    private void StartDialogue()
    {
        dialogueController.StartConversation(dialogueText);
        isConversationStarted = true;
    }

    private void ContinueDialogue()
    {
        if (dialogueController.IsConversationEnded())
        {
            isConversationStarted = false; // Reset for potential future conversations
        }
        else
        {
            dialogueController.DisplayNextParagraph();
        }
    }
}
