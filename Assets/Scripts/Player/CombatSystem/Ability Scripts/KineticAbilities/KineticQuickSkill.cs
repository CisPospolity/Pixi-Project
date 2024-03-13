using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
public class KineticQuickSkill : QuickSkill
{
    [SerializeField]
    protected float range = 5f;
    [SerializeField]
    protected float coneAngle = 45f;
    [SerializeField]
    protected int damage = 2;
    [SerializeField]
    protected ImmobilizingDebuff debuff = null;
    [SerializeField]
    protected float knockbackStrength = 10f;

    public override void Initialize(QuickSkillSO so)
    {
        base.Initialize(so);

        KineticQuickSkillData data = so as KineticQuickSkillData;

        range = data.range;
        coneAngle = data.coneAngle;
        damage = data.damage;
        debuff = data.debuff;
        knockbackStrength = data.knockbackStrength;
    }

    protected override void Awake()
    {
        base.Awake();
        if (playerCombatSystem == null) Destroy(this);
        if (playerCombatSystem.GetQuickSkill() != this)
        {
            if (playerCombatSystem.GetQuickSkill() == null)
            {
                playerCombatSystem.SetExistingQuickSkill(this);
            }
            else
            {
                Destroy(this);
            }
        }

        if(debuff == null || debuff.Duration <= 0f)
        {
            debuff = new ImmobilizingDebuff(1f);
        }
    }

    public override void Execute()
    {
        if (Time.time >= nextAbilityTime)
        {
            GetComponent<Animator>().SetTrigger("KineticQuickSkill");
            debuff.StartTime = Time.time;
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range);

            foreach(var hitCollider in hitEnemies)
            {
                if (hitCollider.gameObject == this.gameObject) continue;
                Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToEnemy) < coneAngle / 2)
                {

                    if (hitCollider.GetComponent<IDamageable>() != null)
                    {
                        hitCollider.GetComponent<IDamageable>().Damage(damage, this.gameObject);
                    }
                    

                    if(hitCollider.GetComponent<EnemyScript>() != null)
                    {
                        directionToEnemy.y = 0;
                        hitCollider.GetComponent<EnemyScript>().PushEnemy(debuff, directionToEnemy * knockbackStrength + new Vector3(0,3f,0), "KineticQuickSkill");
                    } else if(hitCollider.GetComponent<Rigidbody>() != null)
                    {
                        hitCollider.GetComponent<Rigidbody>().AddForce(directionToEnemy * knockbackStrength, ForceMode.VelocityChange);
                    }
                }
            }

            nextAbilityTime = Time.time + abilityCooldown;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        float totalFOV = coneAngle;
        float rayRange = range;
        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);

        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;

        Gizmos.DrawRay(transform.position, leftRayDirection * rayRange);
        Gizmos.DrawRay(transform.position, rightRayDirection * rayRange);

        // To show the arc of the cone
        int segments = 10;
        float segmentAngle = totalFOV / segments;
        for (int i = 0; i <= segments; i++)
        {
            Quaternion segmentRotation = Quaternion.AngleAxis(-halfFOV + (segmentAngle * i), Vector3.up);
            Vector3 segmentDirection = segmentRotation * transform.forward;
            Vector3 previousSegmentDirection = segmentRotation * Quaternion.AngleAxis(-segmentAngle, Vector3.up) * transform.forward;

            if (i > 0)
            {
                Gizmos.DrawLine(transform.position + previousSegmentDirection * rayRange, transform.position + segmentDirection * rayRange);
            }
        }

        // Draw a line forward to indicate the forward direction of the player
        Gizmos.DrawRay(transform.position, transform.forward * rayRange);
    }
}
