using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class ListRandom
{
    public static T GetRandom<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            Debug.LogWarning("List is Empty");
            return default;
        }

        int randomIndex = Random.Range(0, list.Count);
        return list[randomIndex];
    }
}

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private List<CarEnemy.EnemyColor> _enemyColors = new List<CarEnemy.EnemyColor>();
    [SerializeField]
    private List<EnemyStatData> _enemyStatDatas = new List<EnemyStatData>();
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private Quaternion _enemyDefaultRotation = Quaternion.Euler(0, -90f, 0);
    [SerializeField]
    private float _spawnRate = 1;
    [SerializeField]
    private int _defaultSpawnCount = 30;

    private float _spawnTimer;
    private float _spawnPositionX = -100f;
    private int _laneCount;



    private void Start()
    {
        _laneCount = LaneSystem.Instance.LaneCount;
    }


    private void Update()
    {
        _spawnTimer += Time.deltaTime;

        if(_spawnTimer > _spawnRate)
        {
            _spawnTimer = 0;
            int spawnLaneIndex = Random.Range(0, _laneCount);
            float spawnPositonZ = LaneSystem.Instance.GetLanePositionZ(spawnLaneIndex);
            Vector3 spawnPosition = new Vector3(_spawnPositionX, 0.2f, spawnPositonZ);

            GameObject carObject = Instantiate(_enemyPrefab, spawnPosition, _enemyDefaultRotation);
            CarEnemy carEnemy = carObject.GetComponent<CarEnemy>();

            var randomColor = _enemyColors.GetRandom();
            var randomStat = _enemyStatDatas.GetRandom();
            carEnemy.Init(randomColor, randomStat);
        }
    }


}
