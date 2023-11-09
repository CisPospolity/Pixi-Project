using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;

[RequireComponent(typeof(PlayerInputSystem))]
public class PlayerCombatSystem : MonoBehaviour
{
    [Header("Abilities")]
    [SerializeField]
    private PassiveAbility passiveAbility;
    [SerializeField]
    private AttackPassiveAbility attackPassiveAbility;
    [SerializeField]
    private DashAbility dashAbility;
    [SerializeField]
    private QuickSkill quickSkill;
    [SerializeField]
    private StrongSkill strongSkill;

    private PlayerInputSystem playerInputSystem;
    private PlayerComboSystem playerComboSystem;
    public event Action<AttackType> onAttack;
    private void Awake()
    {
        playerInputSystem = GetComponent<PlayerInputSystem>();
        playerComboSystem = GetComponent<PlayerComboSystem>();

        playerInputSystem.onLightAttack += LightAttack;
        playerInputSystem.onHeavyAttack += HeavyAttack;

        playerInputSystem.onDash += DashSkill;
        playerInputSystem.onQuickSkill += QuickSkill;
    }

    public void LightAttack()
    {
        playerComboSystem.LightAttack();
    }

    public void HeavyAttack()
    {
        playerComboSystem.HeavyAttack();
    }

    public void DashSkill()
    {
        if(dashAbility != null)
        {
            dashAbility.Execute();
        }
    }

    public DashAbility GetDashAbility()
    {
        return dashAbility;
    }

    public void SetExistingDashAbility(DashAbility dash)
    {
        dashAbility = dash;
    }

    public void QuickSkill()
    {
        if (quickSkill != null)
        {
            quickSkill.Execute();
        }
    }

    public QuickSkill GetQuickSkill()
    {
        return quickSkill;
    }

    public void SetExistingQuickSkill(QuickSkill qs)
    {
        quickSkill = qs;
    }

}
