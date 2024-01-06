using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField]
    private int health;
    [SerializeField]
    private int maxHealth;

    private int damageTaken;

    public delegate void DamageTakenDelegate(ref int damage);
    public event DamageTakenDelegate OnDamageTaken;

    public void Damage(int damage)
    {
        damageTaken = damage;
        OnDamageTaken?.Invoke(ref damageTaken);
        ApplyDamage(damageTaken);
    }
    public void ApplyDamage(float damage)
    {
        //Trigger OnHit Event
        health -= (int)damage;
    }


    public void Heal(float healAmount)
    {
        health += (int)healAmount;
        if (health > maxHealth) health = maxHealth;
    }
}
