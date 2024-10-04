using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;  // ����Ԥ����
    public float spawnRadiusMin;  // �������ɵ���С����
    public float spawnRadiusMax;  // �������ɵ�������
    public float initialSpawnInterval;  // ��ʼ���ɼ��
    public float minSpawnInterval;  // ��С���ɼ��
    public float spawnAcceleration;  // ÿ�����ɺ���ٵ�ʱ�䣬���Ƽ�������

    public int initialEnemyCountMin = 1;  // ��ʼ���ɵ��˵���С����
    public int initialEnemyCountMax = 3;  // ��ʼ���ɵ��˵��������
    public int maxEnemyCountIncreaseRate = 1;  // ÿ�����ɺ󣬵��������������ӵ��ٶ�

    private float currentSpawnInterval;  // ��ǰ�����ɼ��ʱ��
    private int currentEnemyCountMin;  // ��ǰÿ�����ɵĵ�����С����
    private int currentEnemyCountMax;  // ��ǰÿ�����ɵĵ����������

    public int EnemyCountLimit; //����������������

    void Start()
    {
        // ��ʼ����ǰ���ɼ��Ϊ��ʼ���ɼ��
        currentSpawnInterval = initialSpawnInterval;

        // ��ʼ��������������
        currentEnemyCountMin = initialEnemyCountMin;
        currentEnemyCountMax = initialEnemyCountMax;

        // ��ʼ��������Э��
        StartCoroutine(SpawnEnemiesOverTime());
    }

    // ��������Э��
    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)  // ����ѭ�����ɵ���
        {
            // ����һ������
            SpawnEnemiesBatch();

            // �ȴ���ǰ���ʱ��
            yield return new WaitForSeconds(currentSpawnInterval);

            // ÿ�����ɺ�ӿ������ٶȣ�ֱ���ﵽ��̼��
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnAcceleration);

            // ��ʱ�����ӣ�����������������Ҳ������
            if(currentEnemyCountMax <= EnemyCountLimit)
                currentEnemyCountMax += maxEnemyCountIncreaseRate;
        }
    }

    // ����һ������
    void SpawnEnemiesBatch()
    {
        // �������һ����ǰ��Χ�ڵĵ�������
        int enemyCount = Random.Range(currentEnemyCountMin, currentEnemyCountMax);

        // ���ɶ������
        for (int i = 0; i < enemyCount; i++)
        {
            // ��ȡ�����Χ�����λ��
            Vector3 spawnPosition = GetRandomPositionAroundPlayer();

            // �����λ�����ɵ���
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }

    // ��ȡ�����Χ���������λ��
    Vector3 GetRandomPositionAroundPlayer()
    {
        // ��������Ƕ�
        float angle = Random.Range(0f, Mathf.PI * 2);

        // ����뾶
        float radius = Random.Range(spawnRadiusMin, spawnRadiusMax);

        // ʹ�����Ǻ�������λ��
        Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

        // ��������λ��
        return transform.position + spawnPosition;
    }
}
