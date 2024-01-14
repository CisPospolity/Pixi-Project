using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerAbilities;
using UnityEngine.InputSystem;

public class GravityQuickSkill : QuickSkill
{
    private PlayerInputSystem playerInputSystem;
    private CharacterMovement characterMovement;
    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private float maxThrowRange = 10f;

    private new void Awake()
    {
        playerInputSystem = GetComponent<PlayerInputSystem>();
        characterMovement = GetComponent<CharacterMovement>();
    }

    public override void Execute()
    {
        if (Time.time > nextAbilityTime)
        {
            RaycastHit hit;
            Vector3 hitPos = transform.position;
            if (playerInputSystem.IsGamepad())
            {
                Vector2 rotation = characterMovement.GetRotation();
                if(rotation.sqrMagnitude < 0.05f)
                {
                    rotation = transform.forward;
                }
                Vector3 camForwardDir = Camera.main.transform.forward;
                camForwardDir.y = 0;
                Vector3 camRightDir = Camera.main.transform.right;
                camRightDir.y = 0;
                Vector3 playerDir = camRightDir * rotation.x + camForwardDir * rotation.y;
                hitPos = transform.position + playerDir * maxThrowRange;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (Physics.Raycast(ray, out hit))
                {
                    hitPos = hit.point;
                }
                else
                {
                    // As a fallback, we can assume a default height for the ground plane
                    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
                    float enter = 0.0f;
                    if (groundPlane.Raycast(ray, out enter))
                    {
                        hitPos = ray.GetPoint(enter);
                    }
                    else
                    {
                        // If the raycast still fails, use the character's forward direction
                        hitPos = transform.position + transform.forward * maxThrowRange;
                    }
                }
            }
            float dist = Vector3.Distance(transform.position, hitPos);
            if (dist > maxThrowRange)
            {

                hitPos = hitPos - (hitPos - transform.position).normalized * (dist - maxThrowRange);
            }
            ThrowGrenade(hitPos);
        }
    }

    private void ThrowGrenade(Vector3 targetPos)
    {
        GameObject go = Instantiate(grenadePrefab, transform.position + Vector3.up * 1.5f, Quaternion.identity);
        go.GetComponent<GravityQuickSkillProjectile>().Setup(targetPos);
    }

    
}
