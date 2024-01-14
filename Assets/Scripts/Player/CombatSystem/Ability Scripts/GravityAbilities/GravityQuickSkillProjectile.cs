using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityQuickSkillProjectile : MonoBehaviour
{
    private enum State
    {
        Going,
        Activated,
        Explosion
    }
    private State state;

    private Vector3 targetPos;

    private float startTime;
    private float timeBeforeExplosion = 2f;
    private float durationOfFlight = 0.5f;
    public void Setup(Vector3 TargetPos)
    {
        targetPos = TargetPos;

        startTime = Time.time;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().enabled = false;

    }

    private void Update()
    {
        switch(state)
        {
            case State.Going:
                Go();
                break;
            case State.Activated:
                Activate();
                break;
            case State.Explosion:
                Explosion();
                break;
        }
    }

    //This is initialized each frame
    private void Go()
    {
        float fractionOfJourney = (Time.time - startTime) / durationOfFlight;

        transform.position = Vector3.Lerp(transform.position, targetPos, fractionOfJourney);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f) {
            startTime = Time.time;
            state = State.Activated;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Collider>().enabled = true;

        }
    }

    private void Activate()
    {
        if(Time.time >= startTime + timeBeforeExplosion)
        {
            state = State.Explosion;
        }
    }

    private void Explosion()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 10f);
        foreach(Collider col in colliders)
        {
            if (col.GetComponent<PlayerScript>() != null) continue;

            col.GetComponent<IDamageable>()?.Damage(2);
            col.GetComponent<EnemyScript>()?.AddBuffOrDebuff(new ImmobilizingDebuff(3f), "GravityQuickSkill");
        }

        Destroy(gameObject);
    }
}
