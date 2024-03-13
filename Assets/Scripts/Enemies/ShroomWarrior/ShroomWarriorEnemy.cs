using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using Unity.AI;
using UnityEngine.AI;

public class ShroomWarriorEnemy : EnemyScript
{
    private ShroomWarriorBehaviourTree aiTree;

    [SerializeField]
    private float meleeAttackRange = 2f;
    [SerializeField]
    private float playerFollowRange = 15f;
    [SerializeField]
    private float attackTime = 1f;
    [SerializeField]
    private BoxCollider attackHurtBox;


    private void Start()
    {
        aiTree = GetComponent<ShroomWarriorBehaviourTree>();
    }
    public override Transform FindPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, playerFollowRange);
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Player"))
            {
                return col.transform;
            }
        }
        return null;
    }

    public override void Damage(int damage, GameObject damageSource, bool selfDamage = false)

    {
        if (!selfDamage && damageSource == this.gameObject) return;

        base.Damage(damage, damageSource, selfDamage);
        animator.SetTrigger("gotHit");
    }

    protected override void CheckForImmobilizing()
    {
        base.CheckForImmobilizing();
    }

    public float GetMeleeAttackRange()
    {
        return meleeAttackRange;
    }

    public float GetPlayerFollowRange()
    {
        return playerFollowRange;
    }

    public ref float GetAttackTimer()
    {
        return ref attackTime;
    }

    public void Attack()
    {
        Collider[] cols = Physics.OverlapBox(this.transform.position + attackHurtBox.center, attackHurtBox.size / 2);
        foreach(Collider col in cols)
        {
            col.GetComponent<PlayerScript>()?.Damage(1, this.gameObject);
        }
    }

    public override void Die()
    {
        base.Die();
        aiTree.enabled = false;
        animator.SetTrigger("Die");
        GetComponent<NavMeshAgent>().enabled = false;
        this.enabled = false;
        StartCoroutine(DisableRigidbody());
    }

    IEnumerator DisableRigidbody()
    {
        yield return new WaitUntil(() => GetComponent<Rigidbody>().IsSleeping());
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }
}
