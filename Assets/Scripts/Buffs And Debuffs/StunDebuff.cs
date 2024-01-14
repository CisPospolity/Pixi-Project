using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunDebuff : BuffDebuff
{
    public StunDebuff(float duration)
    {
        Duration = duration;
        StartTime = Time.time;
        type = BuffType.EnemyOnly;
    }

    public override void EvaluateBuffOrDebuff()
    {
        
    }
}
