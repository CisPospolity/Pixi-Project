using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
using UnityEngine.UI;

public class ElectricDash : DashAbility
{
    [SerializeField]
    protected float secondDashWindow = 1f;
    [SerializeField]
    protected float postDashSpeedMultiplier = 1.5f;
    public float speedDecreaseDuration = 1f;

    private bool canSecondDash = false;
    private Image cooldownImage;

    bool showCooldown = true;

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
            canSecondDash = false;
        }
    }

    protected override IEnumerator PerformDash()
    {
        yield return base.PerformDash();

        canSecondDash = true;
        ApplySpeedBuff();
        yield return new WaitForSeconds(secondDashWindow);
        canSecondDash = false;
        if (showCooldown)
        {
            yield return base.UpdateCooldown(cooldownImage);
        }
    }

    private IEnumerator PerformSecondDash()
    {
        yield return StartCoroutine(base.PerformDash());
        ApplySpeedBuff();
        showCooldown = false;
        yield return StartCoroutine(base.UpdateCooldown(cooldownImage));
        showCooldown = true;

    }

    private void ApplySpeedBuff()
    {
        characterMovement.GetSpeedModifierManager().AddOrUpdateSpeedModifier("ElectricDashSpeedBoost", postDashSpeedMultiplier, 1f, speedDecreaseDuration);
    }

    public override IEnumerator UpdateCooldown(Image image)
    {
        cooldownImage = image;
        yield return null;
    }
}
