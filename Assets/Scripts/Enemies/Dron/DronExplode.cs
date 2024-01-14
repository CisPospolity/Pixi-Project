using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
using UnityEngine.AI;

public class DronExplode : Node
{
    private DronEnemy dron;
    private Animator animator;
    private BehaviourTree.Tree tree;
    public DronExplode(DronEnemy dron, Animator animator, BehaviourTree.Tree tree)
    {
        this.dron = dron;
        this.animator = animator;
        this.tree = tree;
    }
    public override NodeState Evaluate()
    {
        if (dron.GetIsExploding()) return NodeState.RUNNING;

        if(animator.GetBool("isJumping"))
        {
            return NodeState.FAILURE;
        }
        dron.Explode();
        tree.enabled = false;
        state = NodeState.RUNNING;
        return state;
    }
}
