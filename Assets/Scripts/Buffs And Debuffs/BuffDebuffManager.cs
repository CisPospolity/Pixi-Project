using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum BuffType
{
    PlayerOnly,
    EnemyOnly,
    Both
}

[Serializable]
public abstract class BuffDebuff
{
    public BuffType type;
    public float InitialMultiplier { get; set; }
    public float FinalMultiplier { get; set; }
    public float StartTime { get; set; }
    [SerializeField]
    public float Duration;

    public abstract void EvaluateBuffOrDebuff();
}

public class BuffDebuffManager
{
    private Dictionary<string, BuffDebuff> buffsAndDebuffs = new Dictionary<string, BuffDebuff>();
    private BuffType availableType = BuffType.Both;

    public BuffDebuffManager(BuffType type)
    {
        availableType = type;
    }
    public void AddOrUpdateBuffOrDebuff(BuffDebuff buffOrDebuff, string id)
    {
        switch(availableType)
        {
            case BuffType.PlayerOnly:
                if(buffOrDebuff.type == BuffType.PlayerOnly || buffOrDebuff.type == BuffType.Both)
                {
                    break;
                }
                return;
            case BuffType.EnemyOnly:
                if (buffOrDebuff.type == BuffType.EnemyOnly || buffOrDebuff.type == BuffType.Both)
                {
                    break;
                }
                return;
            default:
                break;
        }
        if (!buffsAndDebuffs.ContainsKey(id))
        {
            // If the modifier does not exist or has already expired, add a new one
            buffsAndDebuffs.Add(id, buffOrDebuff);
        }
        else
        {
            // If the modifier exists and is active, refresh the start time and duration
            buffsAndDebuffs[id].StartTime = Time.time;
            buffsAndDebuffs[id].Duration = buffOrDebuff.Duration;
        }
    }

    public void EvaluateBuffAndDebuffs()
    {
        foreach(var buff in new List<string>(buffsAndDebuffs.Keys))
        {
            var mod = buffsAndDebuffs[buff];
            if (Time.time - mod.StartTime >= mod.Duration)
            {
                buffsAndDebuffs.Remove(buff);
                continue;
            }
            mod.EvaluateBuffOrDebuff();
        }
    }

    public bool HasSpecificDebuff<T> () where T : BuffDebuff
    {
        return buffsAndDebuffs.Values.OfType<T>().Any();
    }

    public bool HasSpecificDebuffWithout<T, U>() where T : BuffDebuff where U : T
    {
        return buffsAndDebuffs.Values.OfType<T>().Any(buff => buff is T &&!(buff is U));
    }

}
