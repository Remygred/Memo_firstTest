using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // 敌人预制体
    public float spawnRadiusMin;  // 敌人生成的最小距离
    public float spawnRadiusMax;  // 敌人生成的最大距离
    public float initialSpawnInterval;  // 初始生成间隔
    public float minSpawnInterval;  // 最小生成间隔
    public float spawnAcceleration;  // 每次生成后减少的时间，控制加速生成

    public int initialEnemyCountMin = 1;  // 初始生成敌人的最小数量
    public int initialEnemyCountMax = 3;  // 初始生成敌人的最大数量
    public int maxEnemyCountIncreaseRate = 1;  // 每次生成后，敌人生成数量增加的速度

    private float currentSpawnInterval;  // 当前的生成间隔时间
    private int currentEnemyCountMin;  // 当前每次生成的敌人最小数量
    private int currentEnemyCountMax;  // 当前每次生成的敌人最大数量

    public int EnemyCountLimit; //单次最大敌人生成量

    void Start()
    {
        // 初始化当前生成间隔为初始生成间隔
        currentSpawnInterval = initialSpawnInterval;

        // 初始化敌人生成数量
        currentEnemyCountMin = initialEnemyCountMin;
        currentEnemyCountMax = initialEnemyCountMax;

        // 开始敌人生成协程
        StartCoroutine(SpawnEnemiesOverTime());
    }

    // 敌人生成协程
    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)  // 无限循环生成敌人
        {
            // 生成一批敌人
            SpawnEnemiesBatch();

            // 等待当前间隔时间
            yield return new WaitForSeconds(currentSpawnInterval);

            // 每次生成后加快生成速度，直到达到最短间隔
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnAcceleration);

            // 随时间增加，敌人生成数量上限也逐渐增加
            if(currentEnemyCountMax <= EnemyCountLimit)
                currentEnemyCountMax += maxEnemyCountIncreaseRate;
        }
    }

    // 生成一批敌人
    void SpawnEnemiesBatch()
    {
        // 随机生成一个当前范围内的敌人数量
        int enemyCount = Random.Range(currentEnemyCountMin, currentEnemyCountMax);

        // 生成多个敌人
        for (int i = 0; i < enemyCount; i++)
        {
            // 获取玩家周围的随机位置
            Vector3 spawnPosition = GetRandomPositionAroundPlayer();

            // 在随机位置生成敌人
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // 获取玩家周围的随机生成位置
    Vector3 GetRandomPositionAroundPlayer()
    {
        // 生成随机角度
        float angle = Random.Range(0f, Mathf.PI * 2);

        // 随机半径
        float radius = Random.Range(spawnRadiusMin, spawnRadiusMax);

        // 使用三角函数计算位置
        Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

        // 返回生成位置
        return transform.position + spawnPosition;
    }
}
