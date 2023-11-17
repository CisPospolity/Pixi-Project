using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviourTree;
using UnityEngine.AI;
using System;

public class FollowPlayerTask : Node
{
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    private ISpeedProvider speedProvider;
    private ITargetTransformProvider targetTransformProvider;
    public FollowPlayerTask(NavMeshAgent agent, Animator animator, ISpeedProvider speedProvider, ITargetTransformProvider targetTransformProvider)
    {
        this.agent = agent;
        this.animator = animator;
        this.speedProvider = speedProvider;
        this.targetTransformProvider = targetTransformProvider;
    }
    public override NodeState Evaluate()
    {
        player = targetTransformProvider.GetTargetTransform();

        if(agent.enabled == false)
        {
            animator.SetBool("isWalking", false);
            return NodeState.RUNNING;
        }

        if (player != null)
        {
            animator.SetBool("isWalking", true);

            agent.isStopped = false;
            agent.speed = speedProvider.GetSpeed();
            agent.SetDestination(player.position);
        } else
        {
            animator.SetBool("isWalking", false);
            agent.isStopped = true;
        }
        state = NodeState.RUNNING;
        return state;
    }

}
