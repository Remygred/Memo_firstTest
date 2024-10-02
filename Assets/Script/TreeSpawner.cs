using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;  // 树的预制体
    public int numberOfTrees;  // 要生成的树的数量
    public Vector2 spawnAreaMin;  // 生成区域的左下角
    public Vector2 spawnAreaMax;  // 生成区域的右上角

    void Start()
    {
        // 在游戏开始时生成树
        SpawnTrees();
    }

    // 随机生成树
    void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            // 生成一个随机的x和y位置
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // 生成树的预制体
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }
    }

    // 画出生成区域的可视化（调试用）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((spawnAreaMin + spawnAreaMax) / 2, spawnAreaMax - spawnAreaMin);
    }
}
