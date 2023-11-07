using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour, IDamageable, IHealable
{
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;

    public void Damage(float damage)
    {
        health -= damage;
    }

    public void Heal(float healAmount)
    {
        health += healAmount;
        if (health > maxHealth) health = maxHealth;
    }
}
