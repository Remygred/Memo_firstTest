using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;  // 树的预制体
    public int numberOfTrees;  // 要生成的树的数量
    public float spawnRadius;  // 玩家周围生成的范围
    public float relocateDistance;  // 超过这个距离时将树移动
    public float minDistanceFromPlayer = 1f;  // 树生成时与玩家的最小距离
    public float randomDisplacement = 1.5f;  // 增加一点额外的随机偏移量

    private List<GameObject> spawnedTrees = new List<GameObject>();  // 保存所有生成的树
    private Transform player;  // 玩家对象
    private bool isRelocating = false;  // 标记是否正在重新定位

    void Start()
    {
        // 找到玩家的 Transform
        player = transform.parent;

        // 在游戏开始时生成树
        SpawnInitialTrees();
    }

    void Update()
    {
        // 动态检测树的位置，超出范围时重新定位
        if (!isRelocating)  // 防止多次调用
        {
            StartCoroutine(RelocateTreesCoroutine());
        }
    }

    // 在玩家周围生成初始的树
    void SpawnInitialTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector2 spawnPosition = GetRandomPositionAroundPlayer();

            // 确保生成的树不在玩家附近
            while (Vector2.Distance(spawnPosition, player.position) < minDistanceFromPlayer)
            {
                spawnPosition = GetRandomPositionAroundPlayer();  // 如果距离太近，重新生成位置
            }

            GameObject tree = Instantiate(treePrefab, spawnPosition, Quaternion.identity);
            spawnedTrees.Add(tree);  // 保存生成的树
        }
    }

    // 获取玩家周围的随机生成位置，并增加随机偏移量
    Vector2 GetRandomPositionAroundPlayer()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);  // 随机角度
        float radius = Random.Range(0f, spawnRadius) + Random.Range(-randomDisplacement, randomDisplacement);  // 随机半径

        // 计算相对于玩家的位置，并增加随机位移
        Vector2 spawnPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return (Vector2)player.position + spawnPosition;
    }

    // 协程逐步检查并重新定位树的位置，防止性能问题
    IEnumerator RelocateTreesCoroutine()
    {
        isRelocating = true;

        foreach (GameObject tree in spawnedTrees)
        {
            if (tree == null) continue;  // 如果树对象被销毁，跳过

            // 计算树与玩家之间的距离
            float distanceToPlayer = Vector2.Distance(tree.transform.position, player.position);

            // 如果距离超过 relocateDistance，将树移动到玩家周围的随机位置
            if (distanceToPlayer > relocateDistance)
            {
                Vector2 newPosition = GetRandomPositionAroundPlayer();

                // 确保重新定位的树不在玩家附近
                while (Vector2.Distance(newPosition, player.position) < minDistanceFromPlayer)
                {
                    newPosition = GetRandomPositionAroundPlayer();
                }

                tree.transform.position = newPosition;
            }

            yield return null;  // 等待一帧，防止性能卡顿
        }

        isRelocating = false;
    }

    // 画出生成区域的可视化（调试用）
    private void OnDrawGizmosSelected()
    {
        if (player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(player.position, spawnRadius);  // 画出生成范围
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(player.position, relocateDistance);  // 画出重新定位的范围
        }
    }
}
