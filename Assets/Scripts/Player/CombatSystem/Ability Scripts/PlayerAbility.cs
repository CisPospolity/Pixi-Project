using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerAbilities
{
    public abstract class PlayerAbility : MonoBehaviour
    {
        protected PlayerCombatSystem playerCombatSystem;

        protected virtual void Awake()
        {
            playerCombatSystem = GetComponent<PlayerCombatSystem>();
        }
        public abstract void Execute();
    }

    public abstract class PassiveAbility : PlayerAbility
    {

    }

    public abstract class AttackPassiveAbility: PlayerAbility
    {

    }

    public abstract class DashAbility : PlayerAbility
    {
        protected CharacterController characterController;
        protected CharacterMovement characterMovement;

        [SerializeField]
        protected float dashDistance = 5f;
        [SerializeField]
        protected float dashCooldown = 1f;
        [SerializeField]
        protected float dashSpeed = 20f;
        protected float nextDashTime = 0f;

        protected bool isDashing = false;

        protected override void Awake()
        {
            base.Awake();
            if (playerCombatSystem == null) Destroy(this);
            if (playerCombatSystem.GetDashAbility() != this)
            {
                if(playerCombatSystem.GetDashAbility() == null)
                {
                    playerCombatSystem.SetExistingDashAbility(this);
                } else
                {
                    Destroy(this);
                }
            }

            characterController = GetComponent<CharacterController>();
            characterMovement = GetComponent<CharacterMovement>();
        }

        public override void Execute()
        {
            if (Time.time >= nextDashTime && !isDashing)
            {
                StartCoroutine(PerformDash());
            }
        }

        protected virtual IEnumerator PerformDash()
        {
            isDashing = true;
            characterMovement.SetDashBool(true);
            characterMovement.ResetHeightVelocity();
            Vector3 dashDirection = characterMovement.GetRelativeMovementVector().normalized;
            if(dashDirection == Vector3.zero)
            {
                Vector3 tempDir = gameObject.transform.forward;
                dashDirection = new Vector3(tempDir.x, 0, tempDir.z).normalized;
            }

            float startTime = Time.time;

            while(Time.time < startTime + dashDistance / characterController.velocity.magnitude)
            {
                characterController.Move(dashDirection * dashSpeed * Time.deltaTime);
                yield return null;
            }

            nextDashTime = Time.time + dashCooldown;
            isDashing = false;
            characterMovement.SetDashBool(false);
        }
    }

    public abstract class QuickSkill : PlayerAbility
    {
        [SerializeField]
        protected float abilityCooldown = 5f;
        protected float nextAbilityTime = 0f;

    }

    public abstract class StrongSkill : PlayerAbility
    {
        [SerializeField]
        protected float abilityCooldown = 5f;
        protected float nextAbilityTime = 0f;
    }
}
