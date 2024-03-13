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

    public override void Initialize(StrongSkillSO so)
    {
        base.Initialize(so);

        KineticStrongSkillData data = so as KineticStrongSkillData;
        abilityRadius = data.abilityRadius;
        onCastDamage = data.onCastDamage;
        knockbackStrength = data.knockbackStrength;
        onWallHitDamage = data.onWallHitDamage;
    }
    public override void Execute()
    {
        GetComponent<Animator>().SetTrigger("KineticStrong");
        if (Time.time < nextAbilityTime) return;
        nextAbilityTime = Time.time + abilityCooldown;

        Collider[] colliders = Physics.OverlapSphere(transform.position, abilityRadius);
        foreach(var col in colliders)
        {
            col.GetComponent<IDamageable>()?.Damage(onCastDamage, this.gameObject);
            if(col.GetComponent<EnemyScript>() != null)
            {
                Vector3 directionToEnemy = (col.transform.position - transform.position).normalized;
                directionToEnemy.y = 0;
                EnemyScript enemyScript = col.GetComponent<EnemyScript>();
                enemyScript.PushEnemy(
                        (enemyScript.CanBePushed()) ?
                            new KnockbackDebuff(10f, onWallHitDamage, enemyScript.GetComponent<Rigidbody>(), enemyScript) :
                            new ImmobilizingDebuff(1.5f),
                        directionToEnemy * knockbackStrength + new Vector3(0, 3f, 0),
                        "KineticStrongSkillKnockback"
                );
            }
        }
    }
}
