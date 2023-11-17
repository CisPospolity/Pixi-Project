using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyScript : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected float health = 5;
    [SerializeField]
    protected float maxHealth = 5;
    [SerializeField]
    protected float groundCheckDistance = 0.1f; // Distance to check for the ground
    private BuffDebuffManager buffDebuffManager = new BuffDebuffManager(BuffType.EnemyOnly);
    protected NavMeshAgent agent;
    protected Rigidbody enemyRigidbody;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        buffDebuffManager.EvaluateBuffAndDebuffs();
        CheckForImmobilizing();
    }

    protected void CheckForImmobilizing()
    {
        bool immobilized = buffDebuffManager.HasSpecificDebuff<ImmobilizingDebuff>();
        if(immobilized)
        {
            agent.enabled = false;
            enemyRigidbody.isKinematic = false;
            return;
        }
        
        if(IsGrounded())
        {
            enemyRigidbody.isKinematic = true;
            agent.enabled = true;
        }
    }

    public bool GetIfImmobilized() { 
        return buffDebuffManager.HasSpecificDebuff<ImmobilizingDebuff>();
    }

    public void PushEnemy(ImmobilizingDebuff debuff, Vector3 direction, string debuffId)
    {
        buffDebuffManager.AddOrUpdateBuffOrDebuff(debuff, debuffId);
        CheckForImmobilizing();

        enemyRigidbody.AddForce(direction, ForceMode.VelocityChange);
    } 

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    public virtual void Damage(float damage)
    {
        health -= damage;
        if(health < 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {

    }

    public virtual Transform FindPlayer()
    {
        return null;
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; // Choose a color that is visible and distinct
        Vector3 start = transform.position;
        Vector3 end = start - Vector3.up * groundCheckDistance;

        // Draw a line representing the ground check ray
        Gizmos.DrawLine(start, end);
    }
}
