using UnityEngine;

[CreateAssetMenu(fileName = "KineticStrongSkillData", menuName = "Ability System/Kinetic/Kinetic Strong Skill Data")]
public class KineticStrongSkillData : StrongSkillSO
{
    public float abilityRadius = 7f;
    public int onCastDamage = 2;
    public float knockbackStrength = 10f;
    public int onWallHitDamage = 5;
}
