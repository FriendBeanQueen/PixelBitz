using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Header("References")]
    [Tooltip("The player the boss and minions will target.")]
    public Transform player;

    [Header("Minion Prefabs by Role")]
    public GameObject tankMinionPrefab;
    public GameObject rangerMinionPrefab;
    public GameObject healerMinionPrefab;

    [Header("Summoning")]
    [Tooltip("Where minions can spawn from.")]
    public Transform[] summonPoints;
    [Tooltip("Seconds between summon waves.")]
    public float summonInterval = 10f;
    [Tooltip("Maximum total minions allowed at once.")]
    public int maxActiveMinions = 8;

    [Header("Callouts")]
    [Tooltip("Seconds between boss callout changes.")]
    public float calloutInterval = 8f;
    public BossCallout currentCallout = BossCallout.None;

    [Header("Debug")]
    [SerializeField] private List<MinionAI> activeMinions = new List<MinionAI>();

    private float _nextSummonTime;
    private float _nextCalloutTime;

    void Start()
    {
        _nextSummonTime = Time.time + summonInterval;
        _nextCalloutTime = Time.time + calloutInterval;
    }

    void Update()
    {
        if (player == null) return;

        HandleSummoning();
        HandleCallouts();
    }

    // Summoning 

    void HandleSummoning()
    {
        if (Time.time < _nextSummonTime) return;

        // Capacity on active minions
        activeMinions.RemoveAll(m => m == null);
        if (activeMinions.Count >= maxActiveMinions)
        {
            _nextSummonTime = Time.time + summonInterval;
            return;
        }

        DoSummonWave();
        _nextSummonTime = Time.time + summonInterval;
    }

    void DoSummonWave()
    {
        // COMP: 1 tank, 2 rangers, 1 healer
        SpawnMinion(MinionRole.Tank);
        SpawnMinion(MinionRole.Ranger);
        SpawnMinion(MinionRole.Ranger);
        SpawnMinion(MinionRole.Healer);
    }

    void SpawnMinion(MinionRole role)
    {
        if (summonPoints == null || summonPoints.Length == 0) return;

        GameObject prefab = GetPrefabForRole(role);
        if (prefab == null) return;

        Transform point = summonPoints[Random.Range(0, summonPoints.Length)];
        GameObject obj = Instantiate(prefab, point.position, point.rotation);

        MinionAI ai = obj.GetComponent<MinionAI>();
        if (ai != null)
        {
            ai.Initialize(this, player, role);
            activeMinions.Add(ai);
        }
    }

    GameObject GetPrefabForRole(MinionRole role)
    {
        switch (role)
        {
            case MinionRole.Tank: return tankMinionPrefab;
            case MinionRole.Ranger: return rangerMinionPrefab;
            case MinionRole.Healer: return healerMinionPrefab;
            default: return null;
        }
    }

    // Callouts

    void HandleCallouts()
    {
        if (Time.time < _nextCalloutTime) return;

        ChooseAndBroadcastCallout();
        _nextCalloutTime = Time.time + calloutInterval;
    }

    void ChooseAndBroadcastCallout()
    {
        // Very simple demo logic:
        //   - 50% chance to FocusPlayer
        //   - 25% ProtectBoss
        //   - 25% SpreadOut
        int roll = Random.Range(0, 4);
        switch (roll)
        {
            case 0:
            case 1:
                currentCallout = BossCallout.FocusPlayer;
                break;
            case 2:
                currentCallout = BossCallout.ProtectBoss;
                break;
            case 3:
                currentCallout = BossCallout.SpreadOut;
                break;
        }

        // Broadcast to minions
        activeMinions.RemoveAll(m => m == null);
        foreach (var m in activeMinions)
        {
            m.OnBossCallout(currentCallout);
        }

        // trigger log here to show callout to the player.
        Debug.Log($"Boss callout: {currentCallout}");
    }

    // Called by minions

    public void NotifyMinionDied(MinionAI minion)
    {
        activeMinions.Remove(minion);
    }
}


