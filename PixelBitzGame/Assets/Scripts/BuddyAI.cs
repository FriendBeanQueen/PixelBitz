using UnityEngine;
using UnityEngine.AI;

public class BuddyAI : MonoBehaviour
{
    [Header("Follow")]
    public Transform player;
    public float followRadius = 2.0f;         // how close to hover around player
    public float orbitSpeed = 60f;            // degrees/sec for a subtle orbit
    public float rePathRate = 0.1f;           // seconds between SetDestination calls

    [Header("Help Attack")]
    public float assistRadiusFromPlayer = 5f;
    public LayerMask enemyMask;               // set to layer(s) containing enemies, or leave 0 and fallback to tag
    public bool requireLineOfSight = false;
    public LayerMask losBlockers;

    [Header("Shooting")]
    public GameObject bulletPrefab;           
    public Transform firePoint;
    public float fireCooldown = 0.5f;

    [Header("Fallback melee (if no bullet)")]
    public float meleeRange = 1.6f;
    public int meleeDamage = 8;
    public float meleeCooldown = 1.0f;

    private NavMeshAgent agent;
    private float nextPathTime;
    private float nextFireTime;
    private float nextMeleeTime;
    private float orbitAngle; // for soft orbiting

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (player == null)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) player = p.transform;
        }
    }

    void OnEnable()
    {
        if (BuddyCoordinator.Instance != null)
        {
            BuddyCoordinator.Instance.RegisterBuddy(this);
        }
    }

    void OnDisable()
    {
        if (BuddyCoordinator.Instance != null)
        {
            BuddyCoordinator.Instance.UnregisterBuddy(this);
        }
    }

    void Update()
    {
        if (!player) return;

        // 1) FOLLOW: hover/orbit around the player
        orbitAngle += orbitSpeed * Mathf.Deg2Rad * Time.deltaTime;
        Vector3 offset = new Vector3(Mathf.Cos(orbitAngle), 0f, Mathf.Sin(orbitAngle)) * followRadius;
        Vector3 targetPos = player.position + offset;

        if (Time.time >= nextPathTime)
        {
            nextPathTime = Time.time + rePathRate;
            agent.SetDestination(targetPos);
        }

        // 2) ASSIST: find nearest enemy within assistRadiusFromPlayer
        Transform target = FindNearestEnemyAroundPlayer();

        if (target != null)
        {
            // Face target
            Face(target.position);

            // Optional LOS check
            if (!requireLineOfSight || HasLineOfSight(target))
            {
                // Prefer shooting if bullet assigned, else use simple melee
                if (bulletPrefab && firePoint)
                {
                    TryShoot();
                }
                else
                {
                    TryMelee(target);
                }
            }
        }
    }

    Transform FindNearestEnemyAroundPlayer()
    {
        Transform best = null;
        float bestDist = Mathf.Infinity;
        // If enemyMask set, do an OverlapSphere; else fall back to tag search
        if (enemyMask.value != 0)
        {
            var hits = Physics.OverlapSphere(player.position, assistRadiusFromPlayer, enemyMask, QueryTriggerInteraction.Ignore);
            foreach (var h in hits)
            {
                float d = Vector3.Distance(player.position, h.transform.position);
                if (d < bestDist) { bestDist = d; best = h.transform; }
            }
        }
        else
        {

            var enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var e in enemies)
            {
                float d = Vector3.Distance(player.position, e.transform.position);
                if (d <= assistRadiusFromPlayer && d < bestDist)
                {
                    bestDist = d;
                    best = e.transform;
                }
            }
        }
        return best;
    }

    void Face(Vector3 worldPos)
    {
        Vector3 dir = worldPos - transform.position;
        dir.y = 0f;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion look = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, look, 10f * Time.deltaTime);
        }
    }

    bool HasLineOfSight(Transform target)
    {
        Vector3 origin = firePoint ? firePoint.position : (transform.position + Vector3.up * 1.2f);
        Vector3 dir = (target.position + Vector3.up * 0.9f) - origin;
        if (Physics.Raycast(origin, dir.normalized, out var hit, dir.magnitude, losBlockers, QueryTriggerInteraction.Ignore))
            return false;
        return true;
    }

    void TryShoot()
    {
        if (Time.time < nextFireTime) return;
        nextFireTime = Time.time + fireCooldown;
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void TryMelee(Transform target)
    {
        if (Time.time < nextMeleeTime) return;
        float d = Vector3.Distance(transform.position, target.position);
        if (d <= meleeRange)
        {
            nextMeleeTime = Time.time + meleeCooldown;
            var eh = target.GetComponent<EnemyHealth>();
            if (eh) eh.TakeDamage(meleeDamage);
        }
        else
        {
            // step closer if out of melee range
            agent.SetDestination(target.position);
        }
    }

    void OnDrawGizmosSelected()
    {
        // follow radius around buddy (cyan)
        // assist radius around player (yellow)
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, followRadius);
        if (player)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, assistRadiusFromPlayer);
        }
    }
}

