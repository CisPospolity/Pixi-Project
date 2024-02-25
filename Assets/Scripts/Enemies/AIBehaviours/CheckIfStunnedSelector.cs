using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CheckIfStunnedSelector : Node
{
    private EnemyScript enemy;
    private Animator animator;
    public CheckIfStunnedSelector(EnemyScript enemy, Animator enemyAnimator)
    {
        this.enemy = enemy;
        this.animator = enemyAnimator;
    }

    public override NodeState Evaluate()
    {
        if(enemy.GetIfImmobilized())
        {
            animator.SetBool("isWalking", false);
            state = NodeState.FAILURE;
        } else
        {
            state = NodeState.SUCCESS;
        }
        return state;
    }
}
