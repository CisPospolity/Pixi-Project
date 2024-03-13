using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : EnemyScript
{
    [SerializeField]
    private bool debugMode = false;
    public override void Damage(int damage, GameObject damageSource, bool selfDamage = false)
    {
        if (!selfDamage && damageSource == this.gameObject) return;

        base.Damage(damage, damageSource, selfDamage);
        if (debugMode)
        {
            Debug.Log("Got hit for: " + damage + " damage.");
        }
    }

    public override void Die()
    {
        health = maxHealth;
    }
}
