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
    protected float damage = 2f;

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
    }

    public override void Execute()
    {
        if (Time.time >= nextAbilityTime)
        {
            Collider[] hitEnemies = Physics.OverlapSphere(transform.position, range);

            foreach(var hitCollider in hitEnemies)
            {
                if (hitCollider.gameObject == this.gameObject) continue;
                if (hitCollider.GetComponent<IDamageable>() == null) continue;
                Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToEnemy) < coneAngle / 2)
                {
                    hitCollider.GetComponent<IDamageable>().Damage(damage);
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
