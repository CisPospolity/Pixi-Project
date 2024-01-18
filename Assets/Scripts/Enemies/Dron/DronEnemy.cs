using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DronEnemy : EnemyScript
{
    [SerializeField]
    private float playerFindingRange = 25f;
    [SerializeField]
    private float distanceToExplode = 5f;
    [SerializeField]
    private float explosionRange = 10f;
    [SerializeField]
    private AnimationClip explosionAnimation;
    [SerializeField]
    private int explosionDamage;

    private bool isExploding = false;

    public override Transform FindPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, playerFindingRange);
        foreach(Collider col in cols)
        {
            if(col.GetComponent<PlayerScript>())
            {
                return col.transform;
            }
        }
        return null;
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);
        GetComponent<Animator>().SetTrigger("gotHit");
    }

    public ref float GetPlayerRange()
    {
        return ref playerFindingRange;
    }

    public ref float GetDistanceToExplode()
    {
        return ref distanceToExplode;
    }

    public void Explode()
    {
        if(!isExploding)
        {
            isExploding = true;
            StartCoroutine(Explosion());
        }
    }

    public bool GetIsExploding()
    {
        return isExploding;
    }

    private IEnumerator Explosion()
    {
        GetComponent<NavMeshAgent>().enabled = false;
        animator.SetTrigger("explode");
        yield return new WaitForSeconds(explosionAnimation.averageDuration);

        Collider[] hits = Physics.OverlapSphere(transform.position, explosionRange);
        foreach(var hit in hits)
        {
            if(hit.GetComponent<IDamageable>() != null)
            {
                hit.GetComponent<IDamageable>().Damage(explosionDamage);
            }
        }

        Destroy(gameObject);
    }

    protected override void CheckForImmobilizing()
    {
        if (isExploding) return;
        base.CheckForImmobilizing();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red; // Choose a color that is visible and distinct

        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
