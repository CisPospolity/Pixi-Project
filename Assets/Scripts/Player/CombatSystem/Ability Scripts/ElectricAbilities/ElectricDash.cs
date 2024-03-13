using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
public class ElectricDash : DashAbility
{
    [SerializeField]
    protected float secondDashWindow = 1f;
    [SerializeField]
    protected float postDashSpeedMultiplier = 1.5f;
    public float speedDecreaseDuration = 1f;

    private bool canSecondDash = false;

    public override void Initialize(DashSO so)
    {
        base.Initialize(so);

        ElectricDashData data = so as ElectricDashData;
        secondDashWindow = data.secondDashWindow;
        postDashSpeedMultiplier = data.postDashSpeedMultiplier;
        speedDecreaseDuration = data.speedDecreaseDuration;
    }

    public override void Execute()
    {
        base.Execute();

        if(canSecondDash)
        {
            StartCoroutine(PerformSecondDash());
        }
    }

    protected override IEnumerator PerformDash()
    {
        yield return base.PerformDash();

        canSecondDash = true;
        ApplySpeedBuff();
        yield return new WaitForSeconds(secondDashWindow);
        canSecondDash = false;
    }

    private IEnumerator PerformSecondDash()
    {
        yield return StartCoroutine(base.PerformDash());
        ApplySpeedBuff();

    }

    private void ApplySpeedBuff()
    {
        characterMovement.GetSpeedModifierManager().AddOrUpdateSpeedModifier("ElectricDashSpeedBoost", postDashSpeedMultiplier, 1f, speedDecreaseDuration);
    }
}
