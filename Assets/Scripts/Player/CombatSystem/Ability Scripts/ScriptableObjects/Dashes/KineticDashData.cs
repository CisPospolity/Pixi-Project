using UnityEngine;

[CreateAssetMenu(fileName = "KineticDashData", menuName = "Ability System/Kinetic/Kinetic Dash Data")]
public class KineticDashData : DashSO
{
    public float knockbackRadius = 5f;
    public float knockbackStrength = 2f;
    public Vector3 knockbackOffset = Vector3.zero;
}
