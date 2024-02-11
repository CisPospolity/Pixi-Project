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
                new CheckIfStunnedSelector(dron),
                new Selector(new List<Node> {
                    new Sequence(new List<Node>
                    {
                        new CheckIfTargetInRange(dron, this, () => dron.GetDistanceToAttack()),
                        new DronAttack(dron, animator, () => dron.GetAttackTimer(), () => GetTargetTransform(), GetComponent<NavMeshAgent>())
                    }),
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new CheckIfTargetInRange(dron, this, () => dron.GetPlayerRange()),
                            new FollowPlayerTask(GetComponent<NavMeshAgent>(), animator, this, this)
                        }),
                        new StopTask(GetComponent<NavMeshAgent>(), animator)
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
