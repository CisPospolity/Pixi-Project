using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.VFX;

public class FurnanceTrap : MonoBehaviour
{
    [SerializeField]
    private VisualEffect fireVFX; // Assign your fire VFX GameObject in the inspector
    [SerializeField]
    private Collider fireCollider; // Assign your collider in the inspector
    [SerializeField]
    private float duration = 2f;
    [SerializeField]
    private float cooldown = 3f;

    private bool isCooldown = false;

    private void Start()
    {
        fireVFX.Stop();
    }

    private void Update()
    {
        if (!isCooldown)
        {
            StartCoroutine(FireRoutine());
        }
    }

    private IEnumerator FireRoutine()
    {
        isCooldown = true;

        // Enable VFX and collider
        fireVFX.Play();
        fireCollider.enabled = true;

        // Wait for the duration of the fire effect
        yield return new WaitForSeconds(duration);

        // Disable VFX and collider
        fireVFX.Stop();
        fireCollider.enabled = false;

        // Wait for the cooldown period
        yield return new WaitForSeconds(cooldown);

        isCooldown = false;
    }
}
