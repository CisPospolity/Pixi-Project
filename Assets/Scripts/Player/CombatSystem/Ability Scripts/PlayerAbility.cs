using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        protected void OnDestroy()
        {
            StopAllCoroutines();
        }

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
        protected Animator animator;

        [SerializeField]
        protected float dashDistance = 5f;
        [SerializeField]
        protected float dashCooldown = 1f;
        [SerializeField]
        protected float dashSpeed = 20f;
        protected float nextDashTime = 0f;

        protected bool isDashing = false;

        public virtual void Initialize(DashSO so)
        {
            dashDistance = so.dashDistance;
            dashCooldown = so.dashCooldown;
            dashSpeed = so.dashSpeed;
        }
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
            animator = GetComponent<Animator>();
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
            PlayAnimation();
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
            StopAnimation();
            characterMovement.SetDashBool(false);
        }

        protected virtual void PlayAnimation()
        {
            animator.SetBool("isDashing", true);
        }

        protected virtual void StopAnimation()
        {
            animator.SetBool("isDashing", false);
        }


        public virtual IEnumerator UpdateCooldown(Image image)
        {
            float cooldownTimer = 0;
            while(cooldownTimer < dashCooldown)
            {
                cooldownTimer += Time.deltaTime;
                image.fillAmount = 1 - cooldownTimer / dashCooldown;
                yield return null;
            }
        }
    }

    public abstract class QuickSkill : PlayerAbility
    {
        [SerializeField]
        protected float abilityCooldown = 5f;
        protected float nextAbilityTime = 0f;

        public virtual void Initialize(QuickSkillSO data)
        {
            abilityCooldown = data.abilityCooldown;
        }

        public virtual IEnumerator UpdateCooldown(Image image)
        {
            float cooldownTimer = 0;
            while (cooldownTimer < abilityCooldown)
            {
                image.fillAmount = 1 - cooldownTimer / abilityCooldown;
                cooldownTimer += Time.deltaTime;
                yield return null;
            }
        }
    }

    public abstract class StrongSkill : PlayerAbility
    {
        [SerializeField]
        protected float abilityCooldown = 5f;
        protected float nextAbilityTime = 0f;

        public virtual void Initialize(StrongSkillSO data)
        {
            abilityCooldown = data.abilityCooldown;
        }

        public virtual IEnumerator UpdateCooldown(Image image)
        {
            float cooldownTimer = 0;
            while (cooldownTimer < abilityCooldown)
            {
                cooldownTimer += Time.deltaTime;
                image.fillAmount = 1 - cooldownTimer / abilityCooldown;
                yield return null;
            }
        }
    }
}
