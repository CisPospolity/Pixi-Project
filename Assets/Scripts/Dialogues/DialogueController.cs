using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private Image npcSprite;
    [SerializeField]
    private TextMeshProUGUI npcName;
    [SerializeField]
    private TextMeshProUGUI npcDialogueText;

    private Queue<Dialogue> dialogues = new Queue<Dialogue>();
    private Queue<string> paragraphs = new Queue<string>();
    private bool conversationEnded = false;

    private Coroutine typeDialogueCoroutine;
    private const string HTML_ALPHA = "<color=#00000000>";
    private const float MAX_TYPE_TIME = 0.1f;
    private bool isTyping;

    [SerializeField]
    private float typeSpeed = 10;


    public void StartConversation(DialogueText dialogueText)
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        dialogues.Clear();
        paragraphs.Clear();

        foreach (var dialogue in dialogueText.dialogue)
        {
            dialogues.Enqueue(dialogue);
        }

        DisplayNextDialogue();
    }

    public bool IsConversationEnded()
    {
        return conversationEnded;
    }

    private void DisplayNextDialogue()
    {
        if (dialogues.Count == 0)
        {
            EndConversation();
            return;
        }

        // Get next dialogue in queue
        var dialogue = dialogues.Dequeue();

        // Set NPC name and sprite
        npcName.text = dialogue.npcName;
        npcSprite.sprite = dialogue.npcSprite;

        // Enqueue paragraphs for current dialogue
        foreach (var paragraph in dialogue.paragraphs)
        {
            paragraphs.Enqueue(paragraph);
        }

        // Start displaying paragraphs
        DisplayNextParagraph();
    }

    public void DisplayNextParagraph()
    {
        if (paragraphs.Count == 0)
        {
            if (dialogues.Count == 0)
            {
                EndConversation();
            }
            else
            {
                DisplayNextDialogue();
            }
            return;
        }

        if (!isTyping)
        {
            var paragraph = paragraphs.Dequeue();
            if (typeDialogueCoroutine != null)
            {
                StopCoroutine(typeDialogueCoroutine);
            }
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(paragraph));
        }
    }

    private void EndConversation()
    {
        paragraphs.Clear();
        dialogues.Clear();
        conversationEnded = false;
        gameObject.SetActive(false);
    }

    /*private IEnumerator TypeDialogueText(string p)
    {
        float elapsedTime = 0f;

        int charIndex = 0;
        charIndex = Mathf.Clamp(charIndex, 0, p.Length);

        while(charIndex < p.Length)
        {
            elapsedTime += Time.deltaTime * typeSpeed;
            charIndex = Mathf.FloorToInt(elapsedTime);

            npcDialogueText.text = p.Substring(0, charIndex);
            yield return null;
        }
        npcDialogueText = p;
    }*/

    private IEnumerator TypeDialogueText(string p)
    {
        isTyping = true;

        npcDialogueText.text = "";

        string originalText = p;
        string displayedText = "";

        int alphaIndex = 0;

        foreach (char c in p.ToCharArray())
        {
            alphaIndex++;
            npcDialogueText.text = originalText;

            displayedText = npcDialogueText.text.Insert(alphaIndex, HTML_ALPHA);
            npcDialogueText.text = displayedText;

            yield return new WaitForSeconds(MAX_TYPE_TIME / typeSpeed);
        }

        isTyping = false;
    }
}
