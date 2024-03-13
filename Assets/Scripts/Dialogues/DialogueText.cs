using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
public class DialogueText : ScriptableObject
{
    public Dialogue[] dialogue;

}

[Serializable]
public class Dialogue
{
    public Sprite npcSprite;
    public string npcName;
    [TextArea(5, 10)]

    public string[] paragraphs;
}
