using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System;
using UnityEngine.AI;

public class DronAttack : Node
{
    private DronEnemy dron;
    private Animator animator;
    private Func<float> attackTimer;
    private Func<Transform> targetTransform;
    private float attackCounter = 0;
    private NavMeshAgent agent;
    private bool isAttacking = false;
    private float rotationDuration = 0.5f;
    public DronAttack(DronEnemy dron, Animator animator, Func<float> attackTimer, Func<Transform> target, NavMeshAgent agent)
    {
        this.dron = dron;
        this.animator = animator;
        this.attackTimer = attackTimer;
        this.targetTransform = target;
        this.agent = agent;
    }
    public override NodeState Evaluate()
    {
        if(animator.GetBool("isJumping"))
        {
            return NodeState.FAILURE;
        }

        animator.SetBool("isWalking", false);
        if (agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }
        if (!isAttacking)
        {
            animator.SetTrigger("Attack");
            dron.StartCoroutine(RotateTowards());
            isAttacking = true;
            dron.Attack();
        }
        attackCounter += Time.deltaTime;

        if(attackCounter >= attackTimer())
        {
            animator.ResetTrigger("Attack");
            attackCounter = 0f;
            isAttacking = false;
        }
        
        state = NodeState.RUNNING;
        return state;
    }

    IEnumerator RotateTowards()
    {
        Quaternion startRot = dron.transform.rotation;
        Vector3 targetDest = targetTransform().position - dron.transform.position;
        targetDest.y = 0;
        Quaternion endRot = Quaternion.LookRotation(targetDest);
        float elapsedTime = 0;

        while(elapsedTime < rotationDuration)
        {
            float fractionCompleted = elapsedTime / rotationDuration;
            dron.transform.rotation = Quaternion.Lerp(startRot, endRot, fractionCompleted);

            // Increment the elapsed time and wait until the next frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        dron.transform.rotation = endRot;

    }
}
