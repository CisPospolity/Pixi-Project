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
    private bool isDuringAttack = false;
    private Coroutine queueAttack = null;
    private void Awake()
    {
        playerInputSystem = GetComponent<PlayerInputSystem>();
        playerComboSystem = GetComponent<PlayerComboSystem>();

        playerInputSystem.onLightAttack += LightAttack;
        playerInputSystem.onHeavyAttack += HeavyAttack;

        playerInputSystem.onDash += DashSkill;
        playerInputSystem.onQuickSkill += QuickSkill;
        playerInputSystem.onStrongSkill += StrongSkill;
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
        if(dashAbility != null && !isDuringAttack)
        {
            StartCoroutine(ChangeIfAttacking());
            dashAbility.Execute();
        } else if(isDuringAttack && queueAttack == null)
        {
            queueAttack = StartCoroutine(QueueAttack(DashSkill));
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
        if (quickSkill != null && !isDuringAttack)
        {
            StartCoroutine(ChangeIfAttacking());

            quickSkill.Execute();
        }
        else if (isDuringAttack && queueAttack == null)
        {
            queueAttack = StartCoroutine(QueueAttack(QuickSkill));
        }
    }

    public QuickSkill GetQuickSkill()
    {
        return quickSkill;
    }

    public void StrongSkill()
    {
        if (strongSkill != null && !isDuringAttack)
        {
            StartCoroutine(ChangeIfAttacking());
            strongSkill.Execute();
        }
        else if (isDuringAttack && queueAttack == null)
        {
            queueAttack = StartCoroutine(QueueAttack(StrongSkill));
        }
    }

    public StrongSkill GetStrongSkill()
    {
        return strongSkill;
    }

    public void SetExistingQuickSkill(QuickSkill qs)
    {
        quickSkill = qs;
    }

    private IEnumerator ChangeIfAttacking()
    {
        isDuringAttack = true;
        yield return new WaitForSeconds(0.5f);
        isDuringAttack = false;
    }

    private IEnumerator QueueAttack(Action action)
    {
        yield return new WaitUntil(() => !isDuringAttack);
        action?.Invoke();
        queueAttack = null;
    }

}
