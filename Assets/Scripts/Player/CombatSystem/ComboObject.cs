using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newCombo", menuName = "Player Combo System/Combos/New Combo")]
public class Combo : ScriptableObject
{
    public string comboName;
    public List<Attack> comboAttacks;
}
