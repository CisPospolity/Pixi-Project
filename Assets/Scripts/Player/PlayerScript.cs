using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerUIManager))]
public class PlayerScript : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;
    [SerializeField]
    private int permamentShieldValue = 0;

    private List<TemporaryShield> temporaryShields = new List<TemporaryShield>();
    [SerializeField, Tooltip("Invicibilty time after taking damage")]
    private float invicibilityTime = 1.5f;
    private float damageTimer = 0.5f;

    public delegate void HealthUIDelegate(int maxHealth, int health, int shield);
    public event HealthUIDelegate onHealthChange;

    private PlayerUIManager playerUIManager;
    private bool isDead = false;

    private void Awake()
    {
        playerUIManager = GetComponent<PlayerUIManager>();
    }

    void Start()
    {
        onHealthChange?.Invoke(maxHealth, health, CalcuateShields());
        InvokeRepeating("HealPlayer", 20f, 10f);
    }

    public void Damage(int damage, GameObject damageSource, bool selfDamage = false)
    {
        if (!selfDamage && damageSource == this.gameObject) return;

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
        onHealthChange(maxHealth, health, CalcuateShields());

        if(health + CalcuateShields() <= 0)
        {
            Die();
        }
    }

    private void HealPlayer()
    {
        Heal(1);
    }


    public void Heal(float healAmount)
    {
        health += (int)healAmount;
        if (health > maxHealth) health = maxHealth;
        onHealthChange(maxHealth, health, CalcuateShields());

    }

    public void AddTemporaryShield(TemporaryShield shield)
    {
        temporaryShields.Add(shield);
        onHealthChange?.Invoke(maxHealth, health, CalcuateShields());

    }

    public void AddPermamentShield(int value)
    {
        permamentShieldValue += value;
        onHealthChange?.Invoke(maxHealth, health, CalcuateShields());

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
                onHealthChange(maxHealth, health, CalcuateShields());
            }
        }
    }

    private void Die()
    {
        if (isDead) return;
        playerUIManager.ToggleDeathScreen();
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

    public void InstaKill()
    {
        health = 0;
        Die();
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
