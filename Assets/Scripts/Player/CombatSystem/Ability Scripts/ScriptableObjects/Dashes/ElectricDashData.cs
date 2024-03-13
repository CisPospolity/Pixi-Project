using UnityEngine;

[CreateAssetMenu(fileName = "ElectricDashData", menuName = "Ability System/Electric/Electric Dash Data")]
public class ElectricDashData : DashSO
{
    public float secondDashWindow = 1f;
    public float postDashSpeedMultiplier = 1.5f;
    public float speedDecreaseDuration = 1f;
}
