using UnityEngine;

[CreateAssetMenu(fileName = "GravityQuickSkillData", menuName = "Ability System/Gravity/Gravity Quick Skill Data")]
public class GravityQuickSkillData : QuickSkillSO
{
    public GameObject grenadePrefab;
    public float maxThrowRange = 10f;
}
