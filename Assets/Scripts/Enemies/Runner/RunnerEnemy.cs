using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RunnerEnemy : EnemyScript
{
    [SerializeField]
    private float distanceUntilRun = 5f;
    [SerializeField]
    private List<Transform> waypoints = new List<Transform>();
    private int waypointIndex = 0;
    private Transform activeWaypoint;
    private RunnerBehaviourTree aiTree;

    private void Start()
    {
        aiTree = GetComponent<RunnerBehaviourTree>();
        if (waypoints.Count > 0)
        {
            activeWaypoint = waypoints[0];
        }
    }
    public override void Damage(int damage)
    {
        base.Damage(damage);
        animator.SetTrigger("gotHit");
    }

    public override void Die()
    {
        base.Die();
        aiTree.enabled = false;
        animator.SetTrigger("Die");
        GetComponent<NavMeshAgent>().enabled = false;
        this.enabled = false;
        StartCoroutine(DisableRigidbody());
    }

    IEnumerator DisableRigidbody()
    {
        yield return new WaitUntil(() => GetComponent<Rigidbody>().IsSleeping());
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;
    }


    public override Transform FindPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, distanceUntilRun+5);
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Player"))
            {
                return col.transform;
            }
        }
        return null;
    }

    public float GetDistanceUntilRun()
    {
        return distanceUntilRun;
    }

    public Transform GetActiveWaypoint()
    {
        return activeWaypoint;
    }

    public void CalculateNewWaypoint()
    {
        if(waypoints.Count > 2)
        {
            var next = GetNextWaypoint();
            var previous = GetPreviousWaypoint();

            var nextDistToPlayer = Vector3.Distance(FindPlayer().position, next.position);
            Debug.Log("Distance to next " + next.name + ": " + nextDistToPlayer);
            var prevDistToPlayer = Vector3.Distance(FindPlayer().position, previous.position);
            Debug.Log("Distance to prev " + previous.name + ": " + prevDistToPlayer);


            if (nextDistToPlayer>= prevDistToPlayer)
            {
                activeWaypoint = next;
                if (waypointIndex == waypoints.Count - 1)
                {
                    waypointIndex = 0;
                }
                else
                {
                    waypointIndex++;
                }
            } else
            {
                activeWaypoint = previous;
                if (waypointIndex == 0)
                {
                    waypointIndex = waypoints.Count - 1;
                }
                else
                {
                    waypointIndex--;
                }
            }
        }
    }

    private Transform GetNextWaypoint()
    {
        if(waypoints.Count > 1)
        {
            if (waypointIndex == waypoints.Count-1)
            {
                return waypoints[0];
            } else
            {
                return waypoints[waypointIndex +1];
            }
        }
        return waypoints[waypointIndex];
    }

    private Transform GetPreviousWaypoint()
    {
        if (waypoints.Count > 1)
        {
            if (waypointIndex == 0)
            {
                return waypoints[waypoints.Count-1];
            }
            else
            {
                return waypoints[waypointIndex-1];
            }
        }
        return waypoints[waypointIndex];
    }
}
