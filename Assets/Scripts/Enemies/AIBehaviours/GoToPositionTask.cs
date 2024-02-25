using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviourTree;
using System;

public class GoToPositionTask : Node
{
    private NavMeshAgent agent;
    private Func<Transform> newPos;

    private ISpeedProvider speedProvider;
    private Animator animator;
    private bool isMoving = false;

    public GoToPositionTask(NavMeshAgent agent, Func<Transform> targetPos, Animator animator, ISpeedProvider speedProvider)
    {
        this.agent = agent;
        newPos = targetPos;
        this.animator = animator;
        this.speedProvider = speedProvider;
    }

    public override NodeState Evaluate()
    {
        var pos = newPos();
        if (pos == null) return NodeState.FAILURE;
        if (!agent.enabled) return NodeState.FAILURE;
        if (!isMoving)
        {
            agent.SetDestination(pos.position);
            isMoving = true;
            return NodeState.FAILURE;
        }

        if (agent.remainingDistance > 0.2f && Vector3.Distance(agent.transform.position, pos.position) > 0.2f)
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            agent.speed = speedProvider.GetSpeed();
            return NodeState.FAILURE;
        } else
        {
            animator.SetBool("isWalking", false);
            if(agent.isOnNavMesh)
            {
                agent.isStopped = true;
            }
            isMoving = false;

            return NodeState.SUCCESS;
        }

    }
}
