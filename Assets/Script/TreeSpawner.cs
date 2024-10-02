using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;  // ����Ԥ����
    public int numberOfTrees;  // Ҫ���ɵ���������
    public Vector2 spawnAreaMin;  // ������������½�
    public Vector2 spawnAreaMax;  // ������������Ͻ�

    void Start()
    {
        // ����Ϸ��ʼʱ������
        SpawnTrees();
    }

    // ���������
    void SpawnTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            // ����һ�������x��yλ��
            float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
            float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
            Vector2 spawnPosition = new Vector2(randomX, randomY);

            // ��������Ԥ����
            Instantiate(treePrefab, spawnPosition, Quaternion.identity);
        }
    }

    // ������������Ŀ��ӻ��������ã�
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube((spawnAreaMin + spawnAreaMax) / 2, spawnAreaMax - spawnAreaMin);
    }
}
