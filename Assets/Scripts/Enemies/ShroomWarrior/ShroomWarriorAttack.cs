using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using System;
using UnityEngine.AI;

public class ShroomWarriorAttack : Node
{
    private ShroomWarriorEnemy enemy;
    private Animator animator;
    private Func<float> attackTimer;
    private Func<Transform> targetTransform;
    private float attackCounter = 0;
    private NavMeshAgent agent;
    private bool isAttacking = false;
    private float rotationDuration = 0.5f;
    public ShroomWarriorAttack(ShroomWarriorEnemy enemy, Animator animator, Func<float> attackTimer, Func<Transform> target, NavMeshAgent agent)
    {
        this.enemy = enemy;
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
            enemy.StartCoroutine(RotateTowards());
            isAttacking = true;
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
        Quaternion startRot = enemy.transform.rotation;
        Vector3 targetDest = targetTransform().position - enemy.transform.position;
        targetDest.y = 0;
        Quaternion endRot = Quaternion.LookRotation(targetDest);
        float elapsedTime = 0;

        while(elapsedTime < rotationDuration)
        {
            float fractionCompleted = elapsedTime / rotationDuration;
            enemy.transform.rotation = Quaternion.Lerp(startRot, endRot, fractionCompleted);

            // Increment the elapsed time and wait until the next frame
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        enemy.transform.rotation = endRot;

    }
}
