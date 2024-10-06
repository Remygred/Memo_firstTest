using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;  // ����Ԥ����
    public int numberOfTrees;  // Ҫ���ɵ���������
    public float spawnRadius;  // �����Χ���ɵķ�Χ
    public float relocateDistance;  // �����������ʱ�����ƶ�
    public float minDistanceFromPlayer = 1f;  // ������ʱ����ҵ���С����
    public float randomDisplacement = 1.5f;  // ����һ���������ƫ����

    private List<GameObject> spawnedTrees = new List<GameObject>();  // �����������ɵ���
    private Transform player;  // ��Ҷ���
    private bool isRelocating = false;  // ����Ƿ��������¶�λ

    void Start()
    {
        // �ҵ���ҵ� Transform
        player = transform.parent;

        // ����Ϸ��ʼʱ������
        SpawnInitialTrees();
    }

    void Update()
    {
        // ��̬�������λ�ã�������Χʱ���¶�λ
        if (!isRelocating)  // ��ֹ��ε���
        {
            StartCoroutine(RelocateTreesCoroutine());
        }
    }

    // �������Χ���ɳ�ʼ����
    void SpawnInitialTrees()
    {
        for (int i = 0; i < numberOfTrees; i++)
        {
            Vector2 spawnPosition = GetRandomPositionAroundPlayer();

            // ȷ�����ɵ���������Ҹ���
            while (Vector2.Distance(spawnPosition, player.position) < minDistanceFromPlayer)
            {
                spawnPosition = GetRandomPositionAroundPlayer();  // �������̫������������λ��
            }

            GameObject tree = Instantiate(treePrefab, spawnPosition, Quaternion.identity);
            spawnedTrees.Add(tree);  // �������ɵ���
        }
    }

    // ��ȡ�����Χ���������λ�ã����������ƫ����
    Vector2 GetRandomPositionAroundPlayer()
    {
        float angle = Random.Range(0f, Mathf.PI * 2);  // ����Ƕ�
        float radius = Random.Range(0f, spawnRadius) + Random.Range(-randomDisplacement, randomDisplacement);  // ����뾶

        Vector2 spawnPosition = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
        return (Vector2)player.position + spawnPosition;
    }

    IEnumerator RelocateTreesCoroutine()
    {
        isRelocating = true;

        foreach (GameObject tree in spawnedTrees)
        {
            if (tree == null) continue;

            // �����������֮��ľ���
            float distanceToPlayer = Vector2.Distance(tree.transform.position, player.position);

            // ������볬�� relocateDistance�������ƶ��������Χ�����λ��
            if (distanceToPlayer > relocateDistance)
            {
                Vector2 newPosition = GetRandomPositionAroundPlayer();

                // ȷ�����¶�λ����������Ҹ���
                while (Vector2.Distance(newPosition, player.position) < minDistanceFromPlayer)
                {
                    newPosition = GetRandomPositionAroundPlayer();
                }

                tree.transform.position = newPosition;
            }

            yield return null;
        }

        isRelocating = false;
    }

}
