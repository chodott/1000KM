using System.Collections.Generic;
using UnityEngine;

public static class ListRandom
{
    public static T GetRandom<T>(this List<T> list, int range)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("List is Empty");
            return default;
        }

        int randomIndex = Random.Range(0, range-1);
        return list[randomIndex];
    }
}

public class EnemySpawner : MonoBehaviour
{
    #region SerializeField
    [SerializeField]
    private List<CarEnemy.EnemyColor> _enemyColors = new List<CarEnemy.EnemyColor>();
    [SerializeField]
    private List<EnemyStatData> _enemyStatDatas = new List<EnemyStatData>();
    [SerializeField]
    private List<DifficultyTier> _difficultyTiers = new List<DifficultyTier>();
    [SerializeField]
    private GameObject[] _bossPrefabs;
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private Transform _playerTransform;
    [SerializeField]
    private Vector3 _spawnPosition;
    [SerializeField]
    private Vector3 _checkBoxSize;
    [SerializeField]
    private float _enemySpeedMultiplier = 0.3f;

    [SerializeField]
    private int _defaultSpawnCount = 30;
    [SerializeField]
    private float _spawnBudgetLimit = 8f;

    #endregion
    private ObjectPool _objectPool = new ObjectPool();
    private float _spawnBudget = 0f;
    private int _laneCount;
    private int _laneRange;
    private int _spawnBossIndex = 0;

    #region Monobehaviour Callbacks
    private void Start()
    {
        _laneCount = LaneSystem.Instance.LaneCount;
        _laneRange = LaneSystem.Instance.LaneRange;
        _objectPool.CreateDefaultObjects(_enemyPrefab, _defaultSpawnCount);
        GlobalMovementController.Instance.OnReachedMaxDistance += SpawnBoss;
    }

    private void Update()
    {
        DifficultyTier tier =  GetCurrentTier();
        _spawnBudget = Mathf.Min(_spawnBudget + tier.spawnRate * Time.deltaTime, _spawnBudgetLimit);
        if(_spawnBudget < 1f)
        {
            return;
        }

        if (TrySpawnEnemy())
        {
            _spawnBudget -= 1f;
        }
    }
    #endregion

    private DifficultyTier GetCurrentTier()
    {
        float curPlayerSpeed = GlobalMovementController.Instance.GlobalVelocity;
        foreach(var tier in _difficultyTiers)
        {
            if (curPlayerSpeed < tier.minVelocity || curPlayerSpeed  >= tier.maxVelocity)
            {
                continue;
            }
            return tier;
        }
        return _difficultyTiers[_difficultyTiers.Count - 1];
    }

    private bool TrySpawnEnemy()
    {
        List<int> availableLanes = new List<int>();
        for (int index = 0; index < _laneCount; index++)
        {
            int spawnLaneIndex = index - _laneRange;
            float spawnPositionZ = LaneSystem.Instance.GetLanePositionZ(spawnLaneIndex);
            Vector3 spawnPosition = new Vector3(_spawnPosition.x, _spawnPosition.y, spawnPositionZ);

            if (!Physics.CheckBox(spawnPosition, _checkBoxSize, Quaternion.Euler(0, -90f, 0), LayerMask.GetMask("Enemy")))
            {
                availableLanes.Add(spawnLaneIndex);
            }
        }

        if(availableLanes.Count <= 0)
        {
            return false;
        }

        int randomIndex = Random.Range(0, availableLanes.Count);
        int spawnLane = availableLanes[randomIndex];
        //SpawnEnemy(spawnLane);

        return true;

    }

    private void SpawnEnemy(int spawnLaneIndex)
    {
            float spawnPositionZ = LaneSystem.Instance.GetLanePositionZ(spawnLaneIndex);
            Vector3 spawnPosition = new Vector3(_spawnPosition.x, _spawnPosition.y, spawnPositionZ);
            GameObject carObject = _objectPool.GetObject(_enemyPrefab);
            CarEnemy carEnemy = carObject.GetComponent<CarEnemy>();
            var randomColor = _enemyColors.GetRandom(4);
            var randomStat = _enemyStatDatas.GetRandom(3);
            float velocity = GlobalMovementController.Instance.GlobalVelocity * _enemySpeedMultiplier;
            carEnemy.Init(randomColor, randomStat, spawnPosition, velocity, spawnLaneIndex);
     }

    private void SpawnBoss()
    {
        var spawnBossPrefab = _bossPrefabs[_spawnBossIndex];
        GameObject spawnedBoss = Instantiate(spawnBossPrefab, _spawnPosition, Quaternion.Euler(0, -90, 0));
        if (spawnedBoss.TryGetComponent<BossEnemyController>(out var boss)) { /* »ç¿ë */ }
        {
            boss.Init(_playerTransform);
        }
    }
}
