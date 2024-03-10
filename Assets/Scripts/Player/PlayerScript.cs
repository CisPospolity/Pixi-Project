using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int permamentShieldValue = 0;

    private List<TemporaryShield> temporaryShields = new List<TemporaryShield>();

    private float invicibilityTime = 0.5f;
    private float damageTimer = 0.5f;

    public delegate void HealthUIDelegate(int maxHealth, int health, int shield);
    public event HealthUIDelegate OnHealthChange;

    void Start()
    {
        OnHealthChange?.Invoke(maxHealth, health, CalcuateShields());
    }

    public void Damage(int damage)
    {
        if (damageTimer <= invicibilityTime) return;
        damageTimer = 0;
        ApplyDamage(damage);
    }
    public void ApplyDamage(int damage)
    {
        int effectiveDamage = damage;

        foreach(var shield in temporaryShields)
        {
            if (effectiveDamage <= 0) break;

            int absorbed = Mathf.Min(effectiveDamage, shield.shieldAmount);
            shield.shieldAmount -= absorbed;
            effectiveDamage -= absorbed;
        }
        temporaryShields.RemoveAll(shield => shield.shieldAmount <= 0);

        if(effectiveDamage > 0 && permamentShieldValue > 0)
        {
            int absorbed = Mathf.Min(effectiveDamage, permamentShieldValue);
            permamentShieldValue -= absorbed;
            effectiveDamage -= absorbed;
        }
        //Trigger OnHit Event
        health -= effectiveDamage;
        OnHealthChange(maxHealth, health, CalcuateShields());

    }


    public void Heal(float healAmount)
    {
        health += (int)healAmount;
        if (health > maxHealth) health = maxHealth;
        OnHealthChange(maxHealth, health, CalcuateShields());

    }

    public void AddTemporaryShield(TemporaryShield shield)
    {
        temporaryShields.Add(shield);
        OnHealthChange?.Invoke(maxHealth, health, CalcuateShields());

    }

    public void AddPermamentShield(int value)
    {
        permamentShieldValue += value;
        OnHealthChange?.Invoke(maxHealth, health, CalcuateShields());

    }

    private void Update()
    {
        damageTimer += Time.deltaTime;
        UpdateTemporaryShields();
    }

    private void UpdateTemporaryShields()
    {
        for (int i = temporaryShields.Count - 1; i >= 0; i--)
        {
            TemporaryShield shield = temporaryShields[i];
            shield.shieldDuration -= Time.deltaTime;

            if (shield.shieldDuration <= 0 || shield.shieldAmount <= 0)
            {
                temporaryShields.RemoveAt(i);
                OnHealthChange(maxHealth, health, CalcuateShields());
            }
        }
    }

    private int CalcuateShields()
    {
        int shieldAmount = 0;

        foreach(TemporaryShield shield in temporaryShields)
        {
            shieldAmount+= shield.shieldAmount;
        }
        shieldAmount += permamentShieldValue;
        return shieldAmount;
    }
}

public class TemporaryShield
{
    public int shieldAmount;
    public float shieldDuration;

    public TemporaryShield(int shieldAmount, float shieldDuration)
    {
        this.shieldAmount = shieldAmount;
        this.shieldDuration = shieldDuration;
    }
}
