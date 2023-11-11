using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedModifier
{
    public float InitialMultiplier { get; set; }
    public float FinalMultiplier { get; set; }
    public float StartTime { get; set; }
    public float Duration { get; set; }

    public float GetMultiplier()
    {
        float timeSinceStart = Time.time - StartTime;
        if (timeSinceStart >= Duration)
        {
            return FinalMultiplier; // After the duration, return the final multiplier
        }

        // Lerp the multiplier from initial to final over the duration
        return Mathf.Lerp(InitialMultiplier, FinalMultiplier, timeSinceStart / Duration);
    }
}

public class SpeedModifierManager
{
    private Dictionary<string, SpeedModifier> speedModifiers = new Dictionary<string, SpeedModifier>();

    public float CurrentSpeedMultiplier
    {
        get
        {
            float multiplier = 1f;

            foreach (var kvp in new List<string>(speedModifiers.Keys))
            {
                var mod = speedModifiers[kvp];
                multiplier *= mod.GetMultiplier();
                if (Time.time - mod.StartTime >= mod.Duration)
                {
                    speedModifiers.Remove(kvp); // Automatically remove the modifier after its duration
                }
            }
            return multiplier;
        }
    }

    public void AddOrUpdateSpeedModifier(string id, float initialMultiplier, float finalMultiplier, float duration)
    {
        if (!speedModifiers.ContainsKey(id) || speedModifiers[id].StartTime + speedModifiers[id].Duration < Time.time)
        {
            // If the modifier does not exist or has already expired, add a new one
            speedModifiers[id] = new SpeedModifier
            {
                InitialMultiplier = initialMultiplier,
                FinalMultiplier = finalMultiplier,
                StartTime = Time.time,
                Duration = duration
            };
        }
        else
        {
            // If the modifier exists and is active, refresh the start time and duration
            speedModifiers[id].StartTime = Time.time;
            speedModifiers[id].Duration = duration;
        }
    }

}
