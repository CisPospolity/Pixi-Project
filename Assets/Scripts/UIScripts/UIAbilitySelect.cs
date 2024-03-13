using PlayerAbilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class UIAbilitySelect : MonoBehaviour
{
    [Header("Actives")]
    [SerializeField]
    private Image activeDashIcon;
    [SerializeField]
    private Image activeQuickSkillIcon;
    [SerializeField]
    private Image activeStrongSkillIcon;
    [SerializeField]
    private PlayerCombatSystem playerCombat;

    private Dictionary<Type, (Type componentType, string methodName)> abilityMappings = new Dictionary<Type, (Type,string)>()
    {
        //Dashes
        {typeof(ElectricDashData), (typeof(ElectricDash), "ChangeDash") },
        {typeof(KineticDashData), (typeof(KineticDash), "ChangeDash")  },
        {typeof(MagneticDashData),( typeof(MagneticDash), "ChangeDash")  },

        //QuickSkill
        {typeof(KineticQuickSkillData), (typeof(KineticQuickSkill), "ChangeQuickSkill")  },
        {typeof(MagneticQuickSkillData),( typeof(MagneticQuickSkill), "ChangeQuickSkill") },
        {typeof(GravityQuickSkillData), (typeof(GravityQuickSkill), "ChangeQuickSkill") },

        //Strong Skill
        {typeof(KineticStrongSkillData), (typeof(KineticStrongSkill), "ChangeStrongSkill") }

    };

    public void ChangeDash<T, D>(D data) where T : DashAbility where D : DashSO
    {
        playerCombat.ChangeDash<T>(data);
    }
    public void ChangeQuickSkill<T, D>(D data) where T : QuickSkill where D : QuickSkillSO
    {
        playerCombat.ChangeQuickSkill<T>(data);
    }

    public void ChangeStrongSkill<T, D>(D data) where T : StrongSkill where D : StrongSkillSO
    {
        playerCombat.ChangeStrongSkill<T>(data);
    }

    public void ChangeAbility(AbilitySO abilitySO)
    {
        Type soType = abilitySO.GetType();

        if (abilityMappings.TryGetValue(soType, out var mapping))
        {
            Type componentType = mapping.componentType;
            string method = mapping.methodName;

            MethodInfo methodInfo = this.GetType().GetMethod(method, BindingFlags.Public | BindingFlags.Instance);
            if(methodInfo != null)
            {
                MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(new Type[] { componentType, soType });
                genericMethodInfo.Invoke(this, new object[] { abilitySO });
            }
        }
        else
        {
            Debug.LogError($"No ability type mapping found for {soType.Name}");
        }
    }


}
