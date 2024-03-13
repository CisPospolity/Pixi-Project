using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackType
{
    Light,
    Heavy
}
public class PlayerComboSystem : MonoBehaviour
{
    [SerializeField]
    private List<Combo> combos = new List<Combo>();
    [SerializeField]
    private List<Combo> availableCombos;
    private int comboCounter = 0;
    private float comboTimer = 0;
    private float comboLockTimer = 0;

    #region Player Attacks
    [SerializeField]
    private List<BoxCollider> colliderList;
    #endregion

    public event Action<float> onAttackUse;

    private void Start()
    {
        availableCombos = new List<Combo>(combos);
    }

    private void Update()
    {
        if(comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
        } else if (comboTimer <= 0 && comboCounter > 0)
        {
            ResetCombo();
            comboTimer = 0;
        }

        if(comboLockTimer > 0)
        {
            comboLockTimer -= Time.deltaTime;
        }
    }
    public void LightAttack ()
    {
        Attack(AttackType.Light);
    }

    public void HeavyAttack()
    {
        Attack(AttackType.Heavy);
    }

    private void Attack(AttackType type)
    {
        if (comboLockTimer > 0) return;
        if (availableCombos.Count <= 0) ResetCombo();

        List<Combo> toRemove = new List<Combo>();
        for(int i = 0; i < availableCombos.Count; i++)
        {
            if (availableCombos[i].comboAttacks.Count < comboCounter+1)
            {
                toRemove.Add(availableCombos[i]);
                continue;
            }
            if (availableCombos[i].comboAttacks[comboCounter].type != type)
            {
                toRemove.Add(availableCombos[i]);
                continue;
            }
            
        }
        foreach(Combo combo in toRemove)
        {
            availableCombos.Remove(combo);
        }

        if (availableCombos.Count <= 0)
        {
            ResetCombo();
            return;
        }
        BoxCollider col = DetermineCollider(availableCombos[0].comboAttacks[comboCounter]);
        PerformAttack(availableCombos[0].comboAttacks[comboCounter], col);
        GetComponent<Animator>().SetTrigger(availableCombos[0].comboAttacks[comboCounter].triggerName);
        comboTimer = availableCombos[0].comboAttacks[comboCounter].maxNextComboTime;
        comboLockTimer = availableCombos[0].comboAttacks[comboCounter].lockOutTime;
        onAttackUse?.Invoke(availableCombos[0].comboAttacks[comboCounter].lockOutTime);
        comboCounter++;
    }

    private IEnumerator DelayedAttackUse(Attack attack, BoxCollider hitbox, float delay)
    {
        yield return new WaitForSeconds(delay);
        attack.UseAttack(hitbox);
    }

    public void PerformAttack(Attack attack, BoxCollider hitbox)
    {
        attack.InputAttack(hitbox);
        // Start the coroutine to delay the UseAttack
        StartCoroutine(DelayedAttackUse(attack, hitbox, attack.hitboxTime));
    }

    private BoxCollider DetermineCollider(Attack attack)
    {
        foreach(BoxCollider col in colliderList)
        {
            if(col.name == attack.colliderName)
            {
                return col;
            }
        }
        return null;
    }

    public void ResetCombo()
    {
        GetComponent<Animator>().SetTrigger("ResetAA");
        comboCounter = 0;
        availableCombos = new List<Combo>(combos);
    }
}



public abstract class Attack : ScriptableObject
{
    public string attackName;
    public AttackType type;
    public float lockOutTime = 0.2f;
    public float hitboxTime = 0.2f;
    public float maxNextComboTime = 0.5f;
    public string colliderName;
    public string triggerName;
    public event Action onInput;

    public abstract void InputAttack(BoxCollider hitbox = null);
    public abstract void UseAttack(BoxCollider hitbox = null);
}