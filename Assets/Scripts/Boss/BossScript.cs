using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using UnityEngine.VFX;

[RequireComponent(typeof(LineRenderer))]
public class BossScript : EnemyScript
{
    private enum BossState
    {
        IDLE,
        FOLLOW,
        ATTACKING,
        DEAD
    }

    private BossState state = BossState.IDLE;
    private Transform player;
    [SerializeField]
    private float playerFindingRange = 20f;
    private LineRenderer lineRenderer;
    [SerializeField]
    private GameObject afterDeathDialogue;

    [Header("Center laser")]
    [SerializeField]
    private Transform laserTransform;
    [SerializeField]
    private float centerLaserMinCooldown = 10f;
    [SerializeField]
    private float centerLaserMaxCooldown = 15f;
    private float centerLaserCooldown;
    private float centerLaserTimer;
    private bool centerLaserWarning = false;
    private bool shootCenterLaser = false;

    [Header("Side laser")]
    [SerializeField]
    private Transform sideLaserTransform;
    [SerializeField]
    private float sideLaserMinCooldown = 10f;
    [SerializeField]
    private float sideLaserMaxCooldown = 15f;
    private float sideLaserCooldown;
    private float sideLaserTimer;

    [Header("Platforms")]
    [SerializeField]
    private GameObject platformLeft;
    [SerializeField]
    private GameObject platformRight;
    private bool firstPlatformThreshold = false;
    private bool secondPlatformThreshold = false;

    [Header("Leg attack")]
    [SerializeField]
    private GameObject leg;
    [SerializeField]
    private float legMinCooldown = 10f;
    [SerializeField]
    private float legMaxCooldown = 15f;
    private float legCooldown;
    private float legTimer;

    [Header("MasiveLegAttack")]
    [SerializeField]
    private float massiveLegMinCooldown = 20f;
    [SerializeField]
    private float massiveLegMaxCooldown = 35f;
    private float massiveLegCooldown;
    private float massiveLegTimer;

    [Header("Front Shot")]
    [SerializeField]
    private float frontShotMinCooldown = 5f;
    [SerializeField]
    private float frontShotMaxCooldown = 15f;
    private float frontShotCooldown;
    private float frontShotTimer;

    [SerializeField]
    private VisualEffect indicator;

    public override Transform FindPlayer()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, playerFindingRange);
        foreach (Collider col in cols)
        {
            if (col.CompareTag("Player"))
            {
                return col.transform;
            }
        }
        return null;
    }


    private void Start()
    {
        indicator.Stop();
        InvokeRepeating("CheckStates", 0f, 0.2f);
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        centerLaserCooldown = Random.Range(centerLaserMinCooldown, centerLaserMaxCooldown);
        sideLaserCooldown = Random.Range(sideLaserMinCooldown, sideLaserMaxCooldown);
        legCooldown = Random.Range(legMinCooldown, legMaxCooldown);
        massiveLegCooldown = Random.Range(massiveLegMinCooldown, massiveLegMaxCooldown);
        frontShotCooldown = Random.Range(frontShotMinCooldown, frontShotMaxCooldown);
    }

    private void Update()
    {
        CheckForAbilities();
    }

    public override void Die()
    {
        base.Die();
        state = BossState.DEAD;
        animator.SetTrigger("Die");
        this.enabled = false;
        afterDeathDialogue.SetActive(true);
    }

    public override void Damage(int damage, GameObject damageSource, bool selfDamage=false)
    {
        if (state == BossState.DEAD) return;
        if (!selfDamage && damageSource == this.gameObject) return;

        base.Damage(damage, damageSource, selfDamage);

        if((float)health <= ((float)maxHealth * 0.66f) && !firstPlatformThreshold)
        {
            StartCoroutine(DestroyPlatforms());
            firstPlatformThreshold = true;
        }

        if ((float)health <= ((float)maxHealth * 0.33f) && !secondPlatformThreshold)
        {
            StartCoroutine(DestroyPlatforms());
            secondPlatformThreshold = true;
        }
    }

    private void CheckStates()
    {
        switch(state)
        {
            case BossState.IDLE:
                WaitForPlayer();
                break;
            case BossState.FOLLOW:
                FollowPlayer();
                break;
            case BossState.ATTACKING:
                break;
        }
    }

    private void WaitForPlayer()
    {
        player = FindPlayer();
        if (player == null) return;
        if(Vector3.Distance(player.position, transform.position) <= playerFindingRange)
        {
            state = BossState.FOLLOW;
        }
    }

    private void FollowPlayer()
    {
        if (player == null) return;

        StartCoroutine(LookAtPlayer());
    }

    private void CheckForAbilities()
    {
        if (state != BossState.FOLLOW) return;
        centerLaserTimer += Time.deltaTime;
        sideLaserTimer += Time.deltaTime;
        legTimer += Time.deltaTime;
        massiveLegTimer += Time.deltaTime;
        frontShotTimer += Time.deltaTime;

        if(massiveLegTimer >= massiveLegCooldown)
        {
            StartCoroutine(MassiveLegAttack());
            massiveLegTimer = 0;
            massiveLegCooldown = Random.Range(massiveLegMinCooldown, massiveLegMaxCooldown);
            return;
        }

        if(legTimer >= legCooldown)
        {
            StartCoroutine(LegAttack());
            legTimer = 0;
            legCooldown = Random.Range(legMinCooldown, legMaxCooldown);
            return;
        }

        if(centerLaserTimer >= centerLaserCooldown)
        {
            StartCoroutine(LaserAttack());
            centerLaserTimer = 0;
            centerLaserCooldown = Random.Range(centerLaserMinCooldown, centerLaserMaxCooldown);
            return;
        }

        if(sideLaserTimer >= sideLaserCooldown)
        {
            StartCoroutine(SideLaserAttack());
            sideLaserTimer = 0;
            sideLaserCooldown = Random.Range(sideLaserMinCooldown, sideLaserMaxCooldown);
            return;
        }



        if (frontShotTimer >= frontShotCooldown)
        {
            StartCoroutine(FrontShotAttack());
            frontShotTimer = 0;
            frontShotCooldown = Random.Range(frontShotMinCooldown, frontShotMaxCooldown);
            return;
        }
    }

    private IEnumerator FrontShotAttack()
    {
        state = BossState.ATTACKING;
        animator.SetTrigger("FrontShot");
        yield return new WaitForSeconds(0.35f);
        Collider[] hitEnemies = Physics.OverlapSphere(transform.position, 100);

        foreach (var hitCollider in hitEnemies)
        {
            if (hitCollider.gameObject == this.gameObject) continue;
            if (hitCollider.GetComponent<PlayerScript>() != null)
            {
                Vector3 directionToEnemy = (hitCollider.transform.position - transform.position).normalized;
                if (Vector3.Angle(transform.forward, directionToEnemy) < 10 / 2)
                {

                    if (hitCollider.GetComponent<IDamageable>() != null)
                    {
                        hitCollider.GetComponent<IDamageable>().Damage(1, this.gameObject);
                    }
                }
            }
        }
        state = BossState.FOLLOW;

        yield return null;
    }

    private IEnumerator LegAttack()
    {
        state = BossState.ATTACKING;
        leg.SetActive(true);
        leg.transform.position = new Vector3(player.position.x, 70f, player.position.z);
        indicator.transform.position = new Vector3(player.position.x, 2.92f, player.position.z);
        indicator.Play();
        leg.transform.DOMoveY(5, 1f);
        yield return new WaitForSeconds(1.5f);
        leg.SetActive(false);
        state = BossState.FOLLOW;

    }

    private IEnumerator MassiveLegAttack()
    {
        state = BossState.ATTACKING;
        animator.SetBool("MassiveLegAttack", true);
        leg.SetActive(true);
        leg.transform.position = new Vector3(player.position.x, 70f, player.position.z);
        int legAttacks = Random.Range(5, 8);
        for (int i = 0; i < legAttacks; i++)
        {
            leg.transform.DOMoveY(5, 1f);
            indicator.transform.position = new Vector3(player.position.x, 2.92f, player.position.z);
            indicator.Play();
            yield return new WaitForSeconds(1f);
            leg.transform.DOMoveY(70, 1.5f);
            yield return new WaitForSeconds(1.5f);
            leg.transform.position = new Vector3(player.position.x, 70f, player.position.z);
        }
        leg.SetActive(false);
        animator.SetBool("MassiveLegAttack", false);

        state = BossState.FOLLOW;

    }

    private IEnumerator DestroyPlatforms()
    {
        yield return new WaitUntil(() => state == BossState.FOLLOW);
        state = BossState.ATTACKING;
        GameObject selectedPlatform = null;
        if (platformLeft != null && platformRight != null)
        {
            var randomNumber = Random.Range(0, 2);
            if(randomNumber % 2 ==0)
            {
                selectedPlatform = platformLeft;
                platformLeft = null;
            } else
            {
                selectedPlatform = platformRight;

                platformRight = null;
            }
        } else
        {
            if(platformLeft !=null)
            {
                selectedPlatform = platformLeft;
                platformLeft = null;
            } else
            {
                selectedPlatform = platformRight;
                platformRight = null;
            }
        }

        if (selectedPlatform == null) yield break;

        animator.SetTrigger("PlatformDestroy");
        yield return new WaitForSeconds(0.2f);
        selectedPlatform.transform.DOShakePosition(5, 1.5f);
        state = BossState.FOLLOW;
        yield return new WaitForSeconds(5f);
        selectedPlatform.transform.DOLocalMoveY(selectedPlatform.transform.position.y - 50f, 1f);
        yield return new WaitForSeconds(1f);
        Destroy(selectedPlatform);
    }


    public void WarningLaser()
    {
        centerLaserWarning = true;
    }

    public void ShotLaser()
    {
        shootCenterLaser = true;
    }


    private IEnumerator LaserAttack()
    {
        state = BossState.ATTACKING;
        

        animator.SetBool("ShotLaser", true);
        StartCoroutine(LookAtPlayer());
        yield return new WaitUntil(() => centerLaserWarning);
        lineRenderer.enabled = true;

        while (!shootCenterLaser)
        {
            RaycastHit hit;
            Vector3 hitPos = Vector3.zero;
            if (Physics.Raycast(laserTransform.position, player.position - laserTransform.position, out hit))
            {
                if (hit.point != null)
                {
                    hitPos = hit.point;
                }
                else
                {
                    hitPos = laserTransform.position + (player.position - laserTransform.position) * 20f;
                }
            }
            lineRenderer.SetPositions(new Vector3[] { laserTransform.position, hitPos });
            lineRenderer.startWidth = 0.3f;
            lineRenderer.endWidth = 0.3f;
            yield return null;
        }

        float time = 0;
        Vector3 shootDir = player.position - laserTransform.position;
        yield return new WaitForSeconds(1f);
        while (time <1f)
        {
            time += Time.deltaTime;
            RaycastHit hit;
            Vector3 hitPos = Vector3.zero;
            if (Physics.Raycast(laserTransform.position, shootDir, out hit))
            {
                if (hit.point != null)
                {
                    hitPos = hit.point;
                    hit.transform.GetComponent<IDamageable>()?.Damage(1, this.gameObject);
                }
                else
                {
                    hitPos = laserTransform.position + shootDir * 20f;
                }
            }
            lineRenderer.SetPositions(new Vector3[] { laserTransform.position, hitPos });

            lineRenderer.startWidth = 1.2f;
            lineRenderer.endWidth = 1.2f;
            yield return null;
        }

        centerLaserWarning = false;
        shootCenterLaser = false;
        lineRenderer.enabled = false;
        yield return null;
        animator.SetBool("ShotLaser", false);
        state = BossState.FOLLOW;
    }

    private bool startedSideLaser = false;
    private bool endedSideLaser = false;
    public void StartSideLaser()
    {
        startedSideLaser = true;
    }

    public void EndSideLaser()
    {
        endedSideLaser = true;
    }

    private IEnumerator SideLaserAttack()
    {
        state = BossState.ATTACKING;
        animator.SetTrigger("ShotSideLaser");

        /*Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(0, 176.721f, 0) ;
        float time = 0;

        while (time < 0.2f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }*/

        yield return LookAtPlayer();


        yield return new WaitUntil(() => startedSideLaser);

        lineRenderer.enabled = true;
        lineRenderer.startWidth = 1.2f;
        lineRenderer.endWidth = 1.2f;
        while (!endedSideLaser)
        {
            /*Vector3 targetPostition = new Vector3(player.position.x,
                                       this.transform.position.y,
                                       this.transform.position.z);*/
            var rot = Quaternion.LookRotation(sideLaserTransform.position - player.transform.position);

            sideLaserTransform.eulerAngles = new Vector3(rot.x, sideLaserTransform.eulerAngles.y, sideLaserTransform.eulerAngles.z);
            Vector3 hitPos = Vector3.zero;
            RaycastHit hit;
            if (Physics.Raycast(sideLaserTransform.position, sideLaserTransform.forward, out hit))
            {
                if (hit.point != null)
                {
                    hitPos = hit.point;
                    hit.transform.GetComponent<IDamageable>()?.Damage(1, this.gameObject);
                }
                else
                {
                    hitPos = sideLaserTransform.position + sideLaserTransform.forward * 20f;
                }
            }
            else
            {
                hitPos = sideLaserTransform.position + sideLaserTransform.forward * 20f;
            }
            lineRenderer.SetPositions(new Vector3[] { sideLaserTransform.position, hitPos });
            yield return null;
        }
        lineRenderer.enabled = false;
        startedSideLaser = false;
        endedSideLaser = false;
        state = BossState.FOLLOW;
    }

    private IEnumerator LookAtPlayer()
    {
        Vector3 targetDirection = (new Vector3(player.position.x, transform.position.y, player.position.z) - transform.position).normalized;
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.LookRotation(targetDirection);
        float time = 0;

        while (time < 0.2f)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time / 0.2f);
            time += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRotation;
    }
}
