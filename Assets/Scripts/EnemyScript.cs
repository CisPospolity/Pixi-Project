using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected float health = 5;
    [SerializeField]
    protected float maxHealth = 5;


    public virtual void Damage(float damage)
    {
        health -= damage;
        if(health < 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {

    }
}
