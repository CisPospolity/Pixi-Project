using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
using UnityEditor.Playables;

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
    private PlayerUIManager uIManager;
    public event Action<AttackType> onAttack;
    private bool isDuringAttack = false;
    private Coroutine queueAttack = null;
    private void Awake()
    {
        playerInputSystem = GetComponent<PlayerInputSystem>();
        playerComboSystem = GetComponent<PlayerComboSystem>();
        uIManager = GetComponent<PlayerUIManager>();

        playerInputSystem.onLightAttack += LightAttack;
        playerInputSystem.onHeavyAttack += HeavyAttack;

        playerInputSystem.onDash += DashSkill;
        playerInputSystem.onQuickSkill += QuickSkill;
        playerInputSystem.onStrongSkill += StrongSkill;

        playerComboSystem.onAttackUse += LockOut;
    }

    public void LightAttack()
    {
        if (PlayerUIManager.isGamePaused) return;

        if (!isDuringAttack)
        {
            playerComboSystem.LightAttack();

        }
        else if (isDuringAttack && queueAttack == null)
        {
            queueAttack = StartCoroutine(QueueAttack(LightAttack));
        }
    }

    public void HeavyAttack()
    {
        if (PlayerUIManager.isGamePaused) return;

        if (!isDuringAttack)
        {
            playerComboSystem.HeavyAttack();

        }
        else if (isDuringAttack && queueAttack == null)
        {
            queueAttack = StartCoroutine(QueueAttack(HeavyAttack));
        }
    }

    public void DashSkill()
    {
        if (PlayerUIManager.isGamePaused) return;

        if (dashAbility != null && !isDuringAttack)
        {
            StartCoroutine(ChangeIfAttacking());
            dashAbility.Execute();
            dashAbility.StartCoroutine(dashAbility.UpdateCooldown(uIManager.dashIconCooldown));
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
        if (PlayerUIManager.isGamePaused) return;

        if (quickSkill != null && !isDuringAttack)
        {
            StartCoroutine(ChangeIfAttacking());

            quickSkill.Execute();
            quickSkill.StartCoroutine(quickSkill.UpdateCooldown(uIManager.quickSkillIconCooldown));

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
        if (PlayerUIManager.isGamePaused) return;

        if (strongSkill != null && !isDuringAttack)
        {
            StartCoroutine(ChangeIfAttacking());
            strongSkill.Execute();
            strongSkill.StartCoroutine(strongSkill.UpdateCooldown(uIManager.strongSkillIconCooldown));

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

    public void ChangeDash<T>(DashSO data) where T : DashAbility
    {
        if (dashAbility != null)
        {
            Destroy(dashAbility);
            dashAbility = null;
        }

        dashAbility = gameObject.AddComponent<T>();
        dashAbility.Initialize(data);
    }

    public void ChangeQuickSkill<T>(QuickSkillSO data) where T : QuickSkill
    {
        if (quickSkill != null)
        {
            Destroy(quickSkill);
            quickSkill = null;
        }

        quickSkill = gameObject.AddComponent<T>();
        quickSkill.Initialize(data);
    }

    public void ChangeStrongSkill<T>(StrongSkillSO data) where T : StrongSkill
    {
        if (strongSkill != null)
        {
            Destroy(strongSkill);
            strongSkill = null;
        }

        strongSkill = gameObject.AddComponent<T>();
        strongSkill.Initialize(data);
    }

    private void LockOut(float lockOutTime)
    {
        StartCoroutine(ChangeIfAttacking(lockOutTime));
    }

    private IEnumerator ChangeIfAttacking(float lockOutTime = 0.5f)
    {
        isDuringAttack = true;
        yield return new WaitForSeconds(lockOutTime);
        isDuringAttack = false;
    }

    private IEnumerator QueueAttack(Action action)
    {
        yield return new WaitUntil(() => !isDuringAttack);
        action?.Invoke();
        queueAttack = null;
    }

    private void OnDestroy()
    {
        playerInputSystem.onLightAttack -= LightAttack;
        playerInputSystem.onHeavyAttack -= HeavyAttack;

        playerInputSystem.onDash -= DashSkill;
        playerInputSystem.onQuickSkill -= QuickSkill;
        playerInputSystem.onStrongSkill -= StrongSkill;

        playerComboSystem.onAttackUse -= LockOut;
    }

}
