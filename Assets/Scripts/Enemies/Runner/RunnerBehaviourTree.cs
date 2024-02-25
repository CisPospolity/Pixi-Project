using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

[RequireComponent(typeof(RunnerEnemy))]
public class RunnerBehaviourTree : BehaviourTree.Tree, ISpeedProvider, ITargetTransformProvider
{
    private RunnerEnemy runnerEnemy;
    private NavMeshAgent agent;
    private Animator animator;
    private Transform activeWaypoint;
    private void Awake()
    {
        runnerEnemy = GetComponent<RunnerEnemy>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        SetActiveWaypoint();
    }
    public float GetSpeed()
    {
        return runnerEnemy.GetSpeed();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override Node SetupTree()
    {
        Node root = new Sequence(new List<Node> {
                new GoToPositionTask(agent, () => runnerEnemy.GetActiveWaypoint(), animator,  this),
                new Sequence(new List<Node>
                {
                    new CheckIfTargetInRange(runnerEnemy, this, ()=> runnerEnemy.GetDistanceUntilRun()),
                    new RunnerEscape(runnerEnemy, this)
                })
        });

        return root;
    }

    public void SetActiveWaypoint()
    {
        var way = runnerEnemy.GetActiveWaypoint();
        if(way == null)
        {
            activeWaypoint = transform;
        } else
        {
            activeWaypoint = way;
        }
    }

    public Transform GetTargetTransform()
    {
        return runnerEnemy.FindPlayer();
    }
}
