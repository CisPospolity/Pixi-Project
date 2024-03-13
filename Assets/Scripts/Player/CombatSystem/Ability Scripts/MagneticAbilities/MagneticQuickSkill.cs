using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
using UnityEditor;
using System.Diagnostics;

public class MagneticQuickSkill : QuickSkill
{
    [SerializeField]
    private GameObject magneticProjectile;
    [SerializeField]
    private int projectileDamage = 2;
    [SerializeField]
    private Vector3 offsetFromPlayer = new Vector3(0,1.5f,1f);
    [SerializeField]
    private float projectileSpeed = 5f;
    [SerializeField]
    private float pullRadius = 5f;

    [SerializeField]
    private bool debug_offsetGizmo = false;

    public override void Initialize(QuickSkillSO so)
    {
        base.Initialize(so);

        MagneticQuickSkillData data = so as MagneticQuickSkillData;

        magneticProjectile = data.magneticProjectile;
        projectileDamage = data.projectileDamage;
        offsetFromPlayer = data.offsetFromPlayer;
        projectileSpeed = data.projectileSpeed;
        pullRadius = data.pullRadius;
    }
    public override void Execute()
    {
        if(Time.time > nextAbilityTime)
        {
            GameObject proj = Instantiate(magneticProjectile, transform.position + offsetFromPlayer, transform.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * projectileSpeed, ForceMode.VelocityChange);

            proj.GetComponent<MagneticQuickSkillProjectile>().Setup(projectileDamage, pullRadius);

            nextAbilityTime = Time.time + abilityCooldown;
        }
    }

    protected void OnDrawGizmosSelected()
    {
        if(debug_offsetGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + offsetFromPlayer, 0.4f);
        }
    }
}
