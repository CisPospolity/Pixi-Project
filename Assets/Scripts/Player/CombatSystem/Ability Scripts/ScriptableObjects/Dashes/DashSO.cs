using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSO : AbilitySO
{
    public float dashDistance = 5f;
    public float dashCooldown = 1f;
    public float dashSpeed = 20f;
    public float nextDashTime = 0f;
}
