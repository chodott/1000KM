using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.VisualScripting.Member;

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
    private List<DifficultyBand> _difficultyBands = new List<DifficultyBand>();
    [SerializeField]
    private List<float> _difficultySpeedThresholds = new List<float>();
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private Vector3 _spawnPosition;
    [SerializeField]
    private Vector3 _checkBoxSize;
    [SerializeField]
    private float _spawnRateMultiplier = 150;
    [SerializeField]
    private float _enemySpeedMultiplier = 0.3f;

    [SerializeField]
    private int _defaultSpawnCount = 30;

    #endregion
    private ObjectPool _objectPool = new ObjectPool();
    private float _spawnTimer;
    private int _laneCount;
    private int _laneRange;
    private int _difficulty;

    #region Monobehaviour Callbacks
    private void Start()
    {
        _laneCount = LaneSystem.Instance.LaneCount;
        _laneRange = LaneSystem.Instance.LaneRange;
        _objectPool.CreateDefaultObjects(_enemyPrefab, _defaultSpawnCount);
    }


    private void Update()
    {
        _spawnTimer += Time.deltaTime;
        UpdateDifficulty(out float spawnRate);
        if(_spawnTimer > spawnRate)
        {
            _spawnTimer = 0;
            List<int> spawnLanes = GetAvailableSpawnLanes();
            SpawnEnemys(spawnLanes);

        }
    }
    #endregion

    List<T> PickRandom<T>(List<T> list, int n)
    {
        if (n >= list.Count)
        {
            return new List<T>(list);
        }

        List<T> tempList = new List<T>(list);

        for (int i = 0; i < tempList.Count - 1; i++)
        {
            int randIndex = Random.Range(i, tempList.Count);
            T temp = tempList[i];
            tempList[i] = tempList[randIndex];
            tempList[randIndex] = temp;
        }

        return tempList.GetRange(0, n);
    }

    private void UpdateDifficulty(out float spawnRate)
    {
        float curPlayerSpeed = GlobalMovementController.Instance.GlobalVelocity;
        spawnRate = _spawnRateMultiplier / curPlayerSpeed;
        if (curPlayerSpeed >= _difficultySpeedThresholds[_difficulty])
        {
            if (_difficulty >= _difficultySpeedThresholds.Count - 1)
            {
                return;
            }
            _difficulty++;
        }
    }
    private List<int> GetAvailableSpawnLanes()
    {
        List<int> availableLanes = new List<int>();
        for (int index = 0; index < _laneCount; index++)
        {
            int spawnLaneIndex = index - _laneRange;
            float spawnPositionZ = LaneSystem.Instance.GetLanePositionZ(spawnLaneIndex);
            Vector3 spawnPosition = new Vector3(_spawnPosition.x, _spawnPosition.y, spawnPositionZ);

            if(!Physics.CheckBox(spawnPosition, _checkBoxSize,Quaternion.Euler(0,-90f,0), LayerMask.GetMask("Enemy")))
            {
                availableLanes.Add(spawnLaneIndex);
            }
        }
        List<int> spawnLanes = PickRandom(availableLanes, _difficultyBands[_difficulty].spawnLaneCount);
        return spawnLanes;
    }

    private void SpawnEnemys(List<int> spawnLanes)
    {
        foreach (int spawnLaneIndex in spawnLanes)
        {
            float spawnPositionZ = LaneSystem.Instance.GetLanePositionZ(spawnLaneIndex);
            Vector3 spawnPosition = new Vector3(_spawnPosition.x, _spawnPosition.y, spawnPositionZ);
            GameObject carObject = _objectPool.GetObject(_enemyPrefab);
            CarEnemy carEnemy = carObject.GetComponent<CarEnemy>();
            var randomColor = _enemyColors.GetRandom(_difficultyBands[_difficulty].spawnCarColorRange);
            var randomStat = _enemyStatDatas.GetRandom(_difficultyBands[_difficulty].spawnCarTypeRange);
            float velocity = GlobalMovementController.Instance.GlobalVelocity * _enemySpeedMultiplier;
            carEnemy.Init(randomColor, randomStat, spawnPosition, velocity, spawnLaneIndex);
        }
    }

}
