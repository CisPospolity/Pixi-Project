using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;

public class KineticDash : DashAbility
{
    [SerializeField]
    private float knockbackRadius = 5f;
    [SerializeField]
    private float knockbackStrength = 2f;
    [SerializeField]
    private Vector3 knockbackOffset = Vector3.zero;

    [SerializeField]
    private bool debugRadius;

    public override void Initialize(DashSO so)
    {
        base.Initialize(so);

        KineticDashData data = so as KineticDashData;
        knockbackRadius = data.knockbackRadius;
        knockbackStrength = data.knockbackStrength;
        knockbackOffset = data.knockbackOffset;
    }

    protected override IEnumerator PerformDash()
    {
        yield return base.PerformDash();

        Collider[] colliders = Physics.OverlapSphere(transform.position + knockbackOffset, knockbackRadius);
        foreach(Collider col in colliders)
        {
            if(col.GetComponent<EnemyScript>() != null && col.GetComponent<Rigidbody>() != null)
            {
                ImmobilizingDebuff debuff = new ImmobilizingDebuff(0.5f);
                Vector3 pushVector = col.transform.position - transform.position;
                pushVector.y = 1.5f;
                col.GetComponent<EnemyScript>().PushEnemy(debuff, pushVector, "KineticDash");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (debugRadius)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position+knockbackOffset, knockbackRadius);
        }
    }

    protected override void PlayAnimation()
    {
        base.PlayAnimation();
        animator.SetBool("isKineticDash", true);
    }

    protected override void StopAnimation()
    {
        base.StopAnimation();
        animator.SetBool("isKineticDash", false);

    }
}
