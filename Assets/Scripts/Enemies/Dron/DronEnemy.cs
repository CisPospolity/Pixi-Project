using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DronEnemy : EnemyScript
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private float playerFindingRange = 10f;
    public float GetSpeed()
    {
        return speed;
    }

    public override Transform FindPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, playerFindingRange);
        foreach(Collider col in cols)
        {
            if(col.GetComponent<PlayerScript>())
            {
                return col.transform;
            }
        }
        return null;
    }

    public override void Damage(float damage)
    {
        base.Damage(damage);
        GetComponent<Animator>().SetTrigger("gotHit");
    }
}
