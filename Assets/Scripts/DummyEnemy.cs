using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyEnemy : EnemyScript
{
    [SerializeField]
    private bool debugMode = false;
    public override void Damage(float damage)
    {
        base.Damage(damage);
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
