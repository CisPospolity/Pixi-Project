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

    private void Awake()
    {
        enemy = GetComponent<ShroomWarriorEnemy>();
        animator = GetComponent<Animator>();
    }

    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node> {
                new CheckIfStunnedSelector(enemy),
                new Selector(new List<Node> {
                    new Sequence(new List<Node>
                    {
                        new CheckIfTargetInRange(enemy, this, () => enemy.GetMeleeAttackRange()),
                        new ShroomWarriorAttack(enemy, animator, () => enemy.GetAttackTimer(), () => GetTargetTransform(), GetComponent<NavMeshAgent>())
                    }),
                    new Selector(new List<Node> {
                        new Sequence(new List<Node> {
                            new CheckIfTargetInRange(enemy, this, () => enemy.GetPlayerFollowRange()),
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
        return enemy.FindPlayer();
    }

    public float GetSpeed()
    {
        return enemy.GetSpeed();
    }
}
