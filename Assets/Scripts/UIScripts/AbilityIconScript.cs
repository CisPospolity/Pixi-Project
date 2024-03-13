using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityIconScript : MonoBehaviour
{
    [SerializeField]
    private UIAbilitySelect abilitySelect;
    [SerializeField]
    private AbilitySO ability;

    public void ReplaceSkill()
    {
        if (ability == null) return;
        abilitySelect.ChangeAbility(ability);
    }
}
