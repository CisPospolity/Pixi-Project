using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyScript;

public class KnockbackDebuff : ImmobilizingDebuff
{
    public float WallCollisionDamage { get; private set; }
    private Rigidbody enemyRigidbody;
    private EnemyScript es;
    public KnockbackDebuff(float duration, float wallCollisionDamage, Rigidbody enemyRigidbody, EnemyScript es) : base(duration)
    {
        WallCollisionDamage = wallCollisionDamage;
        this.enemyRigidbody = enemyRigidbody;
        this.es = es;
        es.onCollide += CheckForCollisions;
    }

    private void CheckForCollisions(Collision collision)
    {
        foreach(var col in collision.contacts)
        {
            if(col.point.y > enemyRigidbody.transform.position.y)
            {
                enemyRigidbody.GetComponent<EnemyScript>().Damage((int)WallCollisionDamage, null);
                enemyRigidbody.GetComponent<EnemyScript>().AddBuffOrDebuff(new ImmobilizingDebuff(5f), "KineticStrongSkillWallHit");
                enemyRigidbody.velocity = Vector3.zero;
                enemyRigidbody.AddForce(Vector3.up * 5, ForceMode.VelocityChange);
                es.onCollide -= CheckForCollisions;
                Duration = 0f;
                break;
            }
        }
    }

    ~KnockbackDebuff()
    {
        es.onCollide -= CheckForCollisions;
    }
}
