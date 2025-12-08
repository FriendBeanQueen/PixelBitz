using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MinionAI : MonoBehaviour
{
    [Header("Debug")]
    public MinionRole role;
    public BossCallout currentCallout = BossCallout.None;

    [Header("Movement / Combat Settings")]
    public float preferredRange = 8f;      // for rangers
    public float meleeRange = 2f;          // for tanks
    public float moveSpeed = 3.5f;
    public float spreadRadius = 5f;       // so to spread out

    private BossAI _boss;
    private Transform _player;
    private NavMeshAgent _agent;
    private Vector3 _spreadTarget;

    // Simple example timers for "attacks"
    private float _nextAttackTime;
    public float attackCooldown = 1.5f;

    // Initialization

    public void Initialize(BossAI boss, Transform player, MinionRole role)
    {
        _boss = boss;
        _player = player;
        this.role = role;

        if (_agent == null)
            _agent = GetComponent<NavMeshAgent>();

        _agent.speed = moveSpeed;
        ChooseNewSpreadTarget();
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    // Boss communication

    public void OnBossCallout(BossCallout callout)
    {
        currentCallout = callout;

        if (callout == BossCallout.SpreadOut)
        {
            ChooseNewSpreadTarget();
        }
    }

    void ChooseNewSpreadTarget()
    {
        if (_boss == null) return;

        // Random point on a circle around the boss
        Vector2 v = Random.insideUnitCircle.normalized * spreadRadius;
        _spreadTarget = _boss.transform.position + new Vector3(v.x, 0f, v.y);
    }

    // Core loop

    void Update()
    {
        if (_player == null) return;

        switch (currentCallout)
        {
            case BossCallout.FocusPlayer:
                DoFocusPlayer();
                break;
            case BossCallout.ProtectBoss:
                DoProtectBoss();
                break;
            case BossCallout.SpreadOut:
                DoSpreadOut();
                break;
            case BossCallout.GroupUp:
                DoGroupUp();
                break;
            default:
                DoIdleOrDefault();
                break;
        }
    }

    // AI Behaviors

    void DoFocusPlayer()
    {
        switch (role)
        {
            case MinionRole.Tank:
                TankChaseAndMelee();
                break;
            case MinionRole.Ranger:
                RangerKitePlayer();
                break;
            case MinionRole.Healer:
                HealerSupport();
                break;
        }
    }

    void DoProtectBoss()
    {
        if (_boss == null) return;

        switch (role)
        {
            case MinionRole.Tank:
                // Move between player and boss to body-block
                Vector3 dir = (_boss.transform.position - _player.position).normalized;
                Vector3 guardPos = _boss.transform.position + dir * 2f;
                _agent.SetDestination(guardPos);
                TryAttackIfInRange(meleeRange);   // optional poke
                break;

            case MinionRole.Ranger:
                // Stand near boss but facing the player at preferred range
                Vector3 offset = (_player.position - _boss.transform.position).normalized;
                Vector3 shootPos = _boss.transform.position - offset * preferredRange;
                _agent.SetDestination(shootPos);
                TryAttackIfInRange(preferredRange + 1f);
                break;

            case MinionRole.Healer:
                // Stay close to boss and "heal" (WIP)
                _agent.SetDestination(_boss.transform.position);
                TryHealBoss();
                break;
        }
    }

    void DoSpreadOut()
    {
        if (_boss == null) return;

        // Update spread target relative to boss a bit so it follows if boss moves
        Vector3 bossPos = _boss.transform.position;
        Vector3 dirToTarget = (_spreadTarget - bossPos).normalized;
        _spreadTarget = bossPos + dirToTarget * spreadRadius;

        _agent.SetDestination(_spreadTarget);

        // All can still attack if the player is nearby
        TryAttackIfInRange(role == MinionRole.Ranger ? preferredRange + 1f : meleeRange);
    }

    void DoGroupUp()
    {
        if (_boss == null) return;

        _agent.SetDestination(_boss.transform.position);

        // Healers can heal/tanks might still swipe at player if close
        if (role == MinionRole.Healer)
        {
            TryHealBoss();
        }
        else
        {
            TryAttackIfInRange(meleeRange);
        }
    }

    void DoIdleOrDefault()
    {
        // light chase towards player
        _agent.SetDestination(_player.position);
        TryAttackIfInRange(role == MinionRole.Ranger ? preferredRange : meleeRange);
    }

    // Role-specific helpers

    void TankChaseAndMelee()
    {
        float dist = Vector3.Distance(transform.position, _player.position);

        _agent.stoppingDistance = meleeRange * 0.8f;
        _agent.SetDestination(_player.position);

        if (dist <= meleeRange)
        {
            TryAttackIfInRange(meleeRange);
        }
    }

    void RangerKitePlayer()
    {
        float dist = Vector3.Distance(transform.position, _player.position);

        // If too close, back away; if too far, move closer
        Vector3 dir = (transform.position - _player.position).normalized;
        Vector3 desiredPos;

        if (dist < preferredRange * 0.8f)
        {
            desiredPos = _player.position + dir * preferredRange;
        }
        else if (dist > preferredRange * 1.2f)
        {
            desiredPos = _player.position - dir * preferredRange;
        }
        else
        {
            desiredPos = transform.position; // already in good range
        }

        _agent.stoppingDistance = preferredRange * 0.9f;
        _agent.SetDestination(desiredPos);

        TryAttackIfInRange(preferredRange + 0.5f);
    }

    void HealerSupport()
    {
        if (_boss == null) return;

        // stay between boss and player
        Vector3 mid = Vector3.Lerp(_boss.transform.position, _player.position, 0.3f);
        _agent.SetDestination(mid);
        TryHealBoss();
    }

    // "Combat" stubs

    void TryAttackIfInRange(float range)
    {
        if (Time.time < _nextAttackTime) return;

        float dist = Vector3.Distance(transform.position, _player.position);
        if (dist > range) return;

        // Here trigger animation/damage /projectile
        Debug.DrawLine(transform.position, _player.position, Color.red, 0.1f);
        Debug.Log($"{name} ({role}) attacks player under {currentCallout}");

        _nextAttackTime = Time.time + attackCooldown;
    }

    void TryHealBoss()
    {
        if (_boss == null) return;
        if (Time.time < _nextAttackTime) return;

        // Here you’d call into a BossHealth component and add HP
        Debug.Log($"{name} ({role}) heals boss under {currentCallout}");
        _nextAttackTime = Time.time + attackCooldown;
    }

    // Death/cleanup

    public void Die()
    {
        if (_boss != null)
        {
            _boss.NotifyMinionDied(this);
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (_boss != null)
        {
            _boss.NotifyMinionDied(this);
        }
    }
}

