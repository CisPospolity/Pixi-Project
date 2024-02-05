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

    public GoToPositionTask(NavMeshAgent agent, Func<Transform> targetPos, ISpeedProvider speedProvider)
    {
        this.agent = agent;
        newPos = targetPos;
        this.speedProvider = speedProvider;
    }

    public override NodeState Evaluate()
    {
        if (newPos() == null) return NodeState.SUCCESS;

        agent.SetDestination(newPos().position);
        if(agent.remainingDistance > 0.05f)
        {
            
            agent.speed = speedProvider.GetSpeed();
            return NodeState.FAILURE;
        }

        return NodeState.SUCCESS;


    }
}
