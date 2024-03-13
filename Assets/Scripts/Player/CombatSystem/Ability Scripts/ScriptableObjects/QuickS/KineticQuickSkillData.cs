using UnityEngine;

[CreateAssetMenu(fileName = "GravityQuickSkillData", menuName = "Ability System/Kinetic/Kinetic Quick Skill Data")]
public class KineticQuickSkillData : QuickSkillSO
{
    public float range = 5f;
    public float coneAngle = 45f;
    public int damage = 2;
    public ImmobilizingDebuff debuff = null;
    public float knockbackStrength = 10f;
}
