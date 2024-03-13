using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    bool activated = false;
    [SerializeField]
    private GameObject toDestroy;
    private void OnTriggerEnter(Collider other)
    {
        if (activated) return;
        if(other.CompareTag("Player"))
        {
            activated = true;
            StartCoroutine(DestroyAfterTime());
        }
    }

    IEnumerator DestroyAfterTime()
    {
        toDestroy.transform.DOMoveY(toDestroy.transform.position.y - 50f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(toDestroy);

    }
}
