using UnityEngine;
using BehaviourTree;
using System;
using UnityEngine.AI;
using System.Collections.Generic;

[RequireComponent(typeof(DronEnemy))]
public class DronBehaviourTree : BehaviourTree.Tree, ISpeedProvider, ITargetTransformProvider
{
    private DronEnemy dron;
    [SerializeField]
    private Animator animator;
    private void Awake()
    {
        dron=GetComponent<DronEnemy>();
    }

    public float GetSpeed()
    {
        return dron.GetSpeed();
    }

    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node> {
                new CheckIfStunnedSelector(dron),
                new FollowPlayerTask(GetComponent<NavMeshAgent>(), animator, this, this)
                });

        return root;
    }

    public Transform GetTargetTransform()
    {
        return dron.FindPlayer();
    }
}
