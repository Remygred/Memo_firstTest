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

    private float currentSpawnInterval;  // 当前的生成间隔时间

    void Start()
    {
        // 初始化当前生成间隔为初始生成间隔
        currentSpawnInterval = initialSpawnInterval;

        // 开始敌人生成协程
        StartCoroutine(SpawnEnemiesOverTime());
    }

    // 敌人生成协程
    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)  // 无限循环生成敌人
        {
            // 生成敌人
            SpawnEnemy();

            // 等待当前间隔时间
            yield return new WaitForSeconds(currentSpawnInterval);

            // 每次生成后加快生成速度，直到达到最短间隔
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnAcceleration);
        }
    }

    // 生成敌人
    void SpawnEnemy()
    {
        // 获取玩家周围的随机位置
        Vector3 spawnPosition = GetRandomPositionAroundPlayer();

        // 在随机位置生成敌人
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
