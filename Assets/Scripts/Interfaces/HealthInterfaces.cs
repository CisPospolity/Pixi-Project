using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int damage, GameObject damageSource, bool selfDamage = false);
}

public interface IHealable
{
    public void Heal(float healAmount);
}
