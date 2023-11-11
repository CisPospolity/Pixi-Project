using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImmobilizingDebuff : BuffDebuff
{
    public ImmobilizingDebuff(float duration)
    {
        Duration = duration;
        StartTime = Time.time;
        type = BuffType.EnemyOnly;
    }

    public override void EvaluateBuffOrDebuff()
    {

    }
}
