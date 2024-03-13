using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;
    [SerializeField]
    private GameObject damageSource;

    private void OnTriggerEnter(Collider other)
    {
        if (damageSource == null) damageSource = this.gameObject;
        other.GetComponent<IDamageable>()?.Damage(damage, damageSource);
    }
}
