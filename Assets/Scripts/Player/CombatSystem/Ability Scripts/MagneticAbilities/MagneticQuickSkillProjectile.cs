using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class MagneticQuickSkillProjectile : MonoBehaviour
{
    private int damage = 1;
    private float pullRadius = 5f;

    private ImmobilizingDebuff debuff = null;

    public void Setup(int damage, float pullRadius)
    {
        this.damage = damage;
        this.pullRadius = pullRadius;

        if (debuff == null || debuff.Duration <= 0f)
        {
            debuff = new ImmobilizingDebuff(1f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IDamageable>() != null)
        {
            other.GetComponent<IDamageable>().Damage(damage, this.gameObject);

            if(other.GetComponent<EnemyScript>() != null)
            {
                debuff.StartTime = Time.time;
                Collider[] cols = Physics.OverlapSphere(other.transform.position, pullRadius);
                foreach(var col in cols)
                {
                    if (col == other) continue;

                    if(col.GetComponent<EnemyScript>() != null && col.GetComponent<Rigidbody>() != null)
                    {
                        col.GetComponent<EnemyScript>().PushEnemy(debuff, other.transform.position - col.transform.position * 0.9f + new Vector3(0, 3f, 0), "MagneticQuickSkill");
                    }
                }
            }
        }
        Destroy(gameObject);
    }
}
