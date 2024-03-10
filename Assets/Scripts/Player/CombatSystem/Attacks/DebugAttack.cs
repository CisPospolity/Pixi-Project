using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttack", menuName = "Player Combat System/Attack/Debug Attack")]
public class DebugAttack : Attack
{
    [SerializeField]
    private string command = "";
    public override void InputAttack(BoxCollider hitbox = null)
    {
        Debug.Log(command);
    }

    public override void UseAttack(BoxCollider hitbox = null)
    {
        
    }
}
