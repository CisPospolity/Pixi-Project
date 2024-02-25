using BehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ShroomWarriorEnemy))]
public class ShroomWarriorBehaviourTree : BehaviourTree.Tree, ITargetTransformProvider, ISpeedProvider
{
    private ShroomWarriorEnemy enemy;
    [SerializeField]
    private Animator animator;
    private NavMeshAgent agent;

    private void Awake()
    {
        enemy = GetComponent<ShroomWarriorEnemy>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node> {
                new CheckIfStunnedSelector(enemy, animator),
                new Selector(new List<Node> {
                    new Sequence(new List<Node>
                    {
                        new CheckIfTargetInRange(enemy, this, () => enemy.GetMeleeAttackRange()),
                        new ShroomWarriorAttack(enemy, animator, () => enemy.GetAttackTimer(), () => GetTargetTransform(), agent)
                    }),
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new CheckIfTargetInRange(enemy, this, () => enemy.GetPlayerFollowRange()),
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
        return enemy.FindPlayer();
    }

    public float GetSpeed()
    {
        return enemy.GetSpeed();
    }
}
