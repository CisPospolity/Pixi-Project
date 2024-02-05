using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
using UnityEngine.Rendering;

public class KineticStrongSkill : StrongSkill
{
    [SerializeField]
    private float abilityRadius = 7f;
    [SerializeField]
    private int onCastDamage = 2;
    [SerializeField]
    private float knockbackStrength = 10f;
    [SerializeField]
    private int onWallHitDamage = 5;
    public override void Execute()
    {
        if (Time.time < nextAbilityTime) return;
        nextAbilityTime = Time.time + abilityCooldown;

        Collider[] colliders = Physics.OverlapSphere(transform.position, abilityRadius);
        foreach(var col in colliders)
        {
            col.GetComponent<IDamageable>()?.Damage(onCastDamage);
            if(col.GetComponent<EnemyScript>() != null)
            {
                Vector3 directionToEnemy = (col.transform.position - transform.position).normalized;
                directionToEnemy.y = 0;
                EnemyScript enemyScript = col.GetComponent<EnemyScript>();
                enemyScript.PushEnemy(
                        new KnockbackDebuff(10f, onWallHitDamage, enemyScript.GetComponent<Rigidbody>(), enemyScript),
                        directionToEnemy * knockbackStrength + new Vector3(0, 3f, 0),
                        "KineticStrongSkillKnockback"
                );
            }
        }
    }
}
