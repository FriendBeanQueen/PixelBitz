using System.Collections.Generic;
using UnityEngine;

public class BuddyCoordinator : MonoBehaviour
{
    // Simple singleton so buddies can find coordinator
    public static BuddyCoordinator Instance { get; private set; }

    [Header("Assignment Rules")]
    [Tooltip("How many buddies can focus the same enemy at once.")]
    public int maxBuddiesPerEnemy = 1;

    // Buddies in scene tracker
    private readonly List<BuddyAI> _buddies = new List<BuddyAI>();

    // Track which enemy is being handled by which buddies
    // allows multiple buddies per enemy if maxBuddiesPerEnemy increases

    private readonly Dictionary<Transform, List<BuddyAI>> _enemyAssignments
        = new Dictionary<Transform, List<BuddyAI>>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    public void RegisterBuddy(BuddyAI buddy)
    {
        if (buddy == null) return;
        if (!_buddies.Contains(buddy))
            _buddies.Add(buddy);
    }

    public void UnregisterBuddy(BuddyAI buddy)
    {
        if (buddy == null) return;

        _buddies.Remove(buddy);

        // Remove from any enemy assignments
        foreach (var kvp in _enemyAssignments)
        {
            kvp.Value.Remove(buddy);
        }
    }

    // Decide which enemy this buddy should focus, based on enemies near the player + current assignments.

    public Transform GetTargetForBuddy(
        BuddyAI buddy,
        Transform player,
        float assistRadiusFromPlayer,
        LayerMask enemyMask)
    {
        if (buddy == null || player == null)
            return null;

        // 1.) RESET assignments with null enemies/buddies
        CleanupAssignments();

        // 2.) REMOVE any previous assignment for this buddy
        RemoveBuddyFromAssignments(buddy);

        // Find enemies near the player
        Collider[] hits = Physics.OverlapSphere(
            player.position,
            assistRadiusFromPlayer,
            enemyMask
        );

        Transform bestEnemy = null;
        float bestDistSqr = float.MaxValue;

        foreach (Collider hit in hits)
        {
            Transform enemy = hit.transform;
            if (enemy == null) continue;

            // How many buddies are already on this enemy?
            int assignedCount = 0;
            if (_enemyAssignments.TryGetValue(enemy, out var list))
                assignedCount = list.Count;

            // Skip if this enemy is already at capacity
            if (assignedCount >= maxBuddiesPerEnemy)
                continue;

            float dSqr = (enemy.position - player.position).sqrMagnitude;
            if (dSqr < bestDistSqr)
            {
                bestDistSqr = dSqr;
                bestEnemy = enemy;
            }
        }

        if (bestEnemy != null)
        {
            if (!_enemyAssignments.TryGetValue(bestEnemy, out var list))
            {
                list = new List<BuddyAI>();
                _enemyAssignments[bestEnemy] = list;
            }
            if (!list.Contains(buddy))
                list.Add(buddy);
        }

        return bestEnemy;
    }

    private void CleanupAssignments()
    {
        var removeEnemies = new List<Transform>();

        foreach (var kvp in _enemyAssignments)
        {
            var enemy = kvp.Key;
            var list = kvp.Value;

            // Remove null buddies from list
            list.RemoveAll(b => b == null);

            // If enemy disappeared or list is empty, mark for removal
            if (enemy == null || list.Count == 0)
                removeEnemies.Add(enemy);
        }

        foreach (var e in removeEnemies)
        {
            _enemyAssignments.Remove(e);
        }
    }

    private void RemoveBuddyFromAssignments(BuddyAI buddy)
    {
        foreach (var kvp in _enemyAssignments)
        {
            kvp.Value.Remove(buddy);
        }
    }
}
