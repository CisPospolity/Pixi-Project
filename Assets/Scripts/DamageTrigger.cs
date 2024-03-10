using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField]
    private int damage = 1;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>()?.Damage(damage);
    }
}
