using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;

public class MagneticDash : DashAbility
{
    [SerializeField]
    private int baseShieldValue = 2;
    [SerializeField]
    private int shieldValue = 0;

    [SerializeField]
    private float shieldDuration = 5f;
    private float shieldTimer = 0;

    private void Start()
    {
        GetComponent<PlayerScript>().OnDamageTaken += AbsorbDamage;
    }

    private void Update()
    {
        shieldTimer -= Time.deltaTime;
    }

    private void ActivateShield()
    {
        shieldValue = baseShieldValue;
        shieldTimer = shieldDuration;
    }

    private void AbsorbDamage(ref int damage)
    {
        if (IsShieldActive())
        {
            int absorbedDamage = Mathf.Min(damage, shieldValue);
            shieldValue -= absorbedDamage;
            damage -= absorbedDamage;
        }
    }

    protected override IEnumerator PerformDash()
    {
        yield return base.PerformDash();
        ActivateShield();
    }

    private bool IsShieldActive()
    {
        if(shieldTimer > 0)
        {
            if(shieldValue > 0)
            {
                return true;
            }
        }
        return false;
    }
}
