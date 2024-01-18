using System;
using System.Collections;
using System.Collections.Generic;
using BehaviourTree;
using UnityEngine;

public class CheckIfTargetInRange : Node
{
    private EnemyScript enemy;
    private ITargetTransformProvider targetTransformProvider;
    private Func<float> distanceToCheck;
    public CheckIfTargetInRange(EnemyScript enemy, ITargetTransformProvider transformProvider, Func<float> distance)
    {
        this.enemy = enemy;
        targetTransformProvider = transformProvider;
        distanceToCheck = distance;
    }

    public override NodeState Evaluate()
    {
        Transform target = targetTransformProvider.GetTargetTransform();
        if (target == null) return NodeState.FAILURE;
        float distance = Vector3.Distance(enemy.transform.position, target.position);
        if (distance <= distanceToCheck())
        {
            state = NodeState.SUCCESS;
        }
        else
        {
            state = NodeState.FAILURE;
        }
        return state;
    }
}