using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviourTree;
public class CheckIfStunnedSelector : Node
{
    private EnemyScript enemy;
    public CheckIfStunnedSelector(EnemyScript enemy)
    {
        this.enemy = enemy;
    }

    public override NodeState Evaluate()
    {
        if(enemy.GetIfImmobilized())
        {
            enemy.GetComponent<Animator>().SetBool("isWalking", false);
            state = NodeState.FAILURE;
        } else
        {
            state = NodeState.SUCCESS;
        }
        return state;
    }
}
