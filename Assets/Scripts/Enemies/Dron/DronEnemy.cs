using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class DronEnemy : EnemyScript
{
    [SerializeField]
    private float playerFindingRange = 25f;
    [SerializeField]
    private float distanceToAttack = 10f;
    [SerializeField]
    private float attackTime = 1f;
    [SerializeField]
    private float explosionRange = 10f;
    [SerializeField]
    private int laserDamage;
    private LineRenderer lineRenderer;
    [SerializeField]
    private Transform eye;

    private float laserPrep = 0.5f;


    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
    }
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

    public ref float GetDistanceToAttack()
    {
        return ref distanceToAttack;
    }

    public ref float GetAttackTimer()
    {
        return ref attackTime;
    }

    public void Attack()
    {
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        Transform player = FindPlayer();
        var pos = player.transform.position;
        yield return new WaitForSeconds(laserPrep);
        lineRenderer.enabled = true;

        RaycastHit hit;
        if(Physics.Raycast(new Ray(eye.position, pos - transform.position), out hit))
        {
            lineRenderer.SetPositions(new Vector3[] { eye.position, hit.point});
            if(hit.transform == player)
            {
                player.GetComponent<PlayerScript>().ApplyDamage(laserDamage);
            }
        } else
        {
            lineRenderer.SetPositions(new Vector3[] { eye.position, eye.position+ (pos - eye.position)*100f});

        }
        yield return new WaitForSeconds(0.2f);
        lineRenderer.enabled = false;

    }

    protected override void CheckForImmobilizing()
    {
        base.CheckForImmobilizing();
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.red; // Choose a color that is visible and distinct

        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
