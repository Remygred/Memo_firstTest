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

    private float currentSpawnInterval;  // ��ǰ�����ɼ��ʱ��

    void Start()
    {
        // ��ʼ����ǰ���ɼ��Ϊ��ʼ���ɼ��
        currentSpawnInterval = initialSpawnInterval;

        // ��ʼ��������Э��
        StartCoroutine(SpawnEnemiesOverTime());
    }

    // ��������Э��
    IEnumerator SpawnEnemiesOverTime()
    {
        while (true)  // ����ѭ�����ɵ���
        {
            // ���ɵ���
            SpawnEnemy();

            // �ȴ���ǰ���ʱ��
            yield return new WaitForSeconds(currentSpawnInterval);

            // ÿ�����ɺ�ӿ������ٶȣ�ֱ���ﵽ��̼��
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnAcceleration);
        }
    }

    // ���ɵ���
    void SpawnEnemy()
    {
        // ��ȡ�����Χ�����λ��
        Vector3 spawnPosition = GetRandomPositionAroundPlayer();

        // �����λ�����ɵ���
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
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
