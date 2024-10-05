using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrbSpawner : MonoBehaviour
{
    public ObjectPool orbPool;  // ���������
    public int minExperienceOrbs;  // ��С��������
    public int maxExperienceOrbs;  // �����������
    public float spawnRadiusMin;  // ��С���ɰ뾶
    public float spawnRadiusMax;  // ������ɰ뾶

    // ���ɾ�����ĺ���
    public void SpawnExperienceOrbs(Vector3 playerPosition)
    {


        GameObject poolObject = GameObject.FindWithTag("ExpPool");
        if (poolObject != null)
        {
            orbPool = poolObject.GetComponent<ObjectPool>();
        }
        // ������ɾ����������
        int experienceOrbsCount = Random.Range(minExperienceOrbs, maxExperienceOrbs);

        // ѭ������ÿһ��������
        for (int i = 0; i < experienceOrbsCount; i++)
        {
            // �������һ������С�����뾶��Χ�ڵ�λ��
            Vector3 spawnPosition = GetRandomPositionAroundPlayer(playerPosition);

            // �Ӷ���ػ�ȡ�����������ֱ�� Instantiate
            GameObject orb = orbPool.GetObject(); // �Ӷ���ػ�ȡ����
            orb.transform.position = spawnPosition; // ��������λ��
            orb.SetActive(true); // �������
        }
    }

    Vector3 GetRandomPositionAroundPlayer(Vector3 playerPosition)
    {
        // ������ɽǶ�
        float angle = Random.Range(0f, Mathf.PI * 2);

        // ������ɰ뾶
        float radius = Random.Range(spawnRadiusMin, spawnRadiusMax);

        // ʹ�����Ǻ�������������ɵ�λ��
        Vector3 spawnPosition = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;

        // ���������Χ������λ��
        return playerPosition + spawnPosition;
    }
}
