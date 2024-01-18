using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class RunnerEscape : Node
{
    private RunnerEnemy runner;
    private RunnerBehaviourTree bt;

    public RunnerEscape(RunnerEnemy runner, RunnerBehaviourTree bt)
    {
        this.runner = runner;
        this.bt = bt;
    }

    public override NodeState Evaluate()
    {
        runner.CalculateNewWaypoint();
        bt.SetActiveWaypoint();
        return NodeState.SUCCESS;
    }
}
