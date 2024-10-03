using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrbSpawner : MonoBehaviour
{
    public GameObject experienceOrbPrefab;  // 经验球的预制体
    public int minExperienceOrbs;  // 最小生成数量
    public int maxExperienceOrbs;  // 最大生成数量
    public float spawnRadiusMin;  // 最小生成半径
    public float spawnRadiusMax;  // 最大生成半径

    // 生成经验球的函数
    public void SpawnExperienceOrbs(Vector3 playerPosition)
    {
        // 随机生成经验球的数量
        int experienceOrbsCount = Random.Range(minExperienceOrbs, maxExperienceOrbs);

        // 循环生成每一个经验球
        for (int i = 0; i < experienceOrbsCount; i++)
        {
            // 随机生成一个在最小和最大半径范围内的位置
            Vector3 spawnPosition = GetRandomPositionAroundPlayer(playerPosition);

            // 在该位置生成经验球
            Instantiate(experienceOrbPrefab, spawnPosition, Quaternion.identity);
        }
    }

    Vector3 GetRandomPositionAroundPlayer(Vector3 playerPosition)
    {
        // 随机生成角度
        float angle = Random.Range(0f, Mathf.PI * 2);

        // 随机生成半径
        float radius = Random.Range(spawnRadiusMin, spawnRadiusMax);

        // 使用三角函数计算随机生成的位置
        Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

        // 返回玩家周围的生成位置
        return playerPosition + spawnPosition;
    }
}
