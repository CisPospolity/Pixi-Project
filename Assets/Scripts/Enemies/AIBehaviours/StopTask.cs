using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class StopTask : Node
{
    private NavMeshAgent agent;
    private Animator animator;
    public StopTask(NavMeshAgent agent, Animator animator)
    {
        this.agent = agent;
        this.animator = animator;
    }

    public override NodeState Evaluate()
    {
        animator.SetBool("isWalking", false);
        agent.isStopped = true;
        return NodeState.SUCCESS;
    }
}
