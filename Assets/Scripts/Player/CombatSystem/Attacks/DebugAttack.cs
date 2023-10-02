using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newAttack", menuName = "Player Combo System/Attack/Debug Attack")]
public class DebugAttack : Attack
{
    [SerializeField]
    private string command = "";
    public override void InputAttack()
    {
        Debug.Log(command);
    }

    public override void UseAttack()
    {
        throw new System.NotImplementedException();
    }
}
