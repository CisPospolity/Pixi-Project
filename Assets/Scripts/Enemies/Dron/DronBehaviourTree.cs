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
    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        dron=GetComponent<DronEnemy>();
        animator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();
    }

    public float GetSpeed()
    {
        return dron.GetSpeed();
    }

    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node> {
                new CheckIfStunnedSelector(dron, animator),
                new Selector(new List<Node> {
                    new Sequence(new List<Node>
                    {
                        new CheckIfTargetInRange(dron, this, () => dron.GetDistanceToAttack()),
                        new DronAttack(dron, animator, () => dron.GetAttackTimer(), () => GetTargetTransform(), agent)
                    }),
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new CheckIfTargetInRange(dron, this, () => dron.GetPlayerRange()),
                            new FollowPlayerTask(agent, animator, this, this)
                        }),
                        new StopTask(agent, animator)
                    })
                })
        });

        return root;
    }

    public Transform GetTargetTransform()
    {
        return dron.FindPlayer();
    }
}
