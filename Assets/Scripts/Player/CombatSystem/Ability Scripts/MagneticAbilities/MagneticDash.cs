using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;

public class MagneticDash : DashAbility
{
    [SerializeField]
    private int baseShieldValue = 2;

    [SerializeField]
    private float shieldDuration = 5f;

    private void ActivateShield()
    {
        GetComponent<PlayerScript>().AddTemporaryShield(new TemporaryShield(baseShieldValue, shieldDuration));
    }

    protected override IEnumerator PerformDash()
    {
        yield return base.PerformDash();
        ActivateShield();
    }
}
