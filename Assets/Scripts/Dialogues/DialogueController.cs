using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI npcName;
    [SerializeField]
    private TextMeshProUGUI npcDialogueText;

    private Queue<string> paragraphs = new Queue<string>();
    private bool conversationEnded = false;

    private Coroutine typeDialogueCoroutine;
    private const string HTML_ALPHA = "<color=#00000000>";
    private const float MAX_TYPE_TIME = 0.1f;
    private bool isTyping;

    [SerializeField]
    private float typeSpeed = 10;

    public void DisplayNextParagraph(DialogueText dialogueText)
    {
        if(paragraphs.Count == 0)
        {
            if(!conversationEnded)
            {
                StartConversation(dialogueText);
            } else
            {
                EndConversation();
                return;
            }
        }


        if(!isTyping)
        {
            var p = paragraphs.Dequeue();
            typeDialogueCoroutine = StartCoroutine(TypeDialogueText(p));
        }

        if(paragraphs.Count == 0)
        {
            conversationEnded = true;
        }
    }

    private void StartConversation(DialogueText dialogueText)
    {
        if(!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        npcName.text = dialogueText.speakerName;

        for(int i =0; i< dialogueText.paragraphs.Length; i++)
        {
            paragraphs.Enqueue(dialogueText.paragraphs[i]);
        }
    }

    private void EndConversation()
    {
        paragraphs.Clear();
        conversationEnded = false;
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
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
