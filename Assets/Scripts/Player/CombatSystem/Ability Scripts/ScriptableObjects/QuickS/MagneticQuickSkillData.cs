using UnityEngine;

[CreateAssetMenu(fileName = "MagneticQuickSkillData", menuName = "Ability System/Magnetic/Magnetic Quick Skill Data")]
public class MagneticQuickSkillData : QuickSkillSO
{
    public GameObject magneticProjectile;
    public int projectileDamage = 2;
    public Vector3 offsetFromPlayer = new Vector3(0, 3.5f, 2f);
    public float projectileSpeed = 5f;
    public float pullRadius = 5f;
}
