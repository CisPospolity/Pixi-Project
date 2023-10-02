using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputSystem))]
public class PlayerCombatSystem : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
    private PlayerComboSystem playerComboSystem;
    public event Action<AttackType> onAttack;
    private void Awake()
    {
        playerInputSystem = GetComponent<PlayerInputSystem>();
        playerComboSystem = GetComponent<PlayerComboSystem>();

        playerInputSystem.onLightAttack += LightAttack;
        playerInputSystem.onHeavyAttack += HeavyAttack;
    }

    public void LightAttack()
    {
        playerComboSystem.LightAttack();
    }

    public void HeavyAttack()
    {
        playerComboSystem.HeavyAttack();
    }

}
