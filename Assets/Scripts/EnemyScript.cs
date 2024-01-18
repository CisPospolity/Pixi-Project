using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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

    protected Animator animator;

    private float jumpStartTime;
    private bool jumping = false;
    private float journeyLength;
    private Vector3 endPos;
    float jumpHeight = 1f;

    [SerializeField]
    private bool canBePushed = true;
    [SerializeField]
    private bool canBeImmobilized = true;


    [SerializeField]
    protected float speed = 5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        buffDebuffManager.EvaluateBuffAndDebuffs();
        CheckForImmobilizing();
        DisableAutoTraverse();
    }

    protected virtual void DisableAutoTraverse()
    {
        /*if (agent.isOnOffMeshLink)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            if(!jumping)
            {
                endPos = data.endPos + Vector3.up * agent.baseOffset;
                jumping = true;
                jumpStartTime = Time.time;
                journeyLength = Vector3.Distance(data.startPos, endPos);

                jumpHeight = Mathf.Abs(endPos.y - agent.transform.position.y) + 1f;
            }

            //calculate the final point of the link

            float distCovered = (Time.time - jumpStartTime) * agent.speed;
            float fractionOfJourney = distCovered / journeyLength;
            Vector3 newPos = Vector3.MoveTowards(agent.transform.position, new Vector3(endPos.x, agent.transform.position.y, endPos.z), agent.speed * Time.deltaTime);

            float y = Mathf.Sin(Mathf.PI * fractionOfJourney) * jumpHeight;
            newPos.y = Mathf.Lerp(agent.transform.position.y, endPos.y, fractionOfJourney) + y;
            //Move the agent to the end point
            agent.transform.position = newPos;

            //when the agent reach the end point you should tell it, and the agent will "exit" the link and work normally after that
            if (Vector3.Distance(agent.transform.position, endPos) < 0.1f)
            {
                agent.CompleteOffMeshLink();
                jumping = false;
            }
        }*/

        if (agent.isOnOffMeshLink)
        {
                animator.SetBool("isJumping", true);
            if(!jumping)
            {
                jumping = true;
            }
            OffMeshLinkData data = agent.currentOffMeshLinkData;

            //calculate the final point of the link
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;

            //Move the agent to the end point
            agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);

            //when the agent reach the end point you should tell it, and the agent will "exit" the link and work normally after that
            if (Vector3.Distance(agent.transform.position, endPos) < 0.1f)
            {
                agent.CompleteOffMeshLink();
                jumping = false;
                animator.SetBool("isJumping", false);

            }
        }
    }

    protected virtual void CheckForImmobilizing()
    {
        if (!canBeImmobilized) return;
        bool immobilized = buffDebuffManager.HasSpecificDebuff<ImmobilizingDebuff>();
        if(immobilized)
        {

            enemyRigidbody.isKinematic = false;
            if(agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
            agent.enabled = false;
            return;
        }
        
        if(IsGrounded() && agent.enabled == false && !immobilized)
        {
            enemyRigidbody.isKinematic = true;
            
            agent.enabled = true;
            if (agent.isOnNavMesh)
            {
                agent.isStopped = false;
            }
        }
    }

    public bool GetIfImmobilized() { 
        return buffDebuffManager.HasSpecificDebuff<ImmobilizingDebuff>();
    }

    public void PushEnemy(ImmobilizingDebuff debuff, Vector3 direction, string debuffId)
    {
        buffDebuffManager.AddOrUpdateBuffOrDebuff(debuff, debuffId);
        agent.enabled = false;
        CheckForImmobilizing();

        if (canBePushed)
        {
            enemyRigidbody.AddForce(direction, ForceMode.VelocityChange);
        }
    }

    public void AddBuffOrDebuff(BuffDebuff effect, string effectID)
    {
        buffDebuffManager.AddOrUpdateBuffOrDebuff(effect, effectID);
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundCheckDistance);
    }

    public virtual void Damage(int damage)
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


    public virtual float GetSpeed()
    {
        return speed;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue; // Choose a color that is visible and distinct
        Vector3 start = transform.position;
        Vector3 end = start - Vector3.up * groundCheckDistance;

        // Draw a line representing the ground check ray
        Gizmos.DrawLine(start, end);
    }
}
