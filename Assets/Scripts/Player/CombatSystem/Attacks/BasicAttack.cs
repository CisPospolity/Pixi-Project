using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack/BasicAttack")]
public class BasicAttack : Attack
{
    public LayerMask mask;
    public int attackDamage;
    public override void InputAttack(BoxCollider hitbox = null)
    {
        
    }

    public override void UseAttack(BoxCollider hitbox = null)
    {
        if (hitbox == null) return;
        Collider[] cols = Physics.OverlapBox(hitbox.transform.position, hitbox.size/2, hitbox.transform.rotation, mask);
        foreach(Collider col in cols)
        {
            if (col.GetComponent<PlayerScript>() != null) return;
            col.GetComponent<IDamageable>()?.Damage(attackDamage, null) ;
        }
    }
}
