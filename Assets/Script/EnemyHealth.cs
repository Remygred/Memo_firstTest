using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int Hp;
    public ExperienceOrbSpawner spawner;  // ������������
    public ObjectPool enemyPool;  // ���ö����
    public string PoolTag;

    private void Start()
    {
        GameObject poolObject = GameObject.FindWithTag(PoolTag);
        if (poolObject != null)
        {
            enemyPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    // ��������
    public void TakeDamage(int damage, bool isBoom)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Die(isBoom);
        }
    }

    // ��������
    void Die(bool isBoom)
    {
        // ���þ�����������
        spawner = GetComponent<ExperienceOrbSpawner>();
        if (spawner != null && !isBoom)
        {
            // ������˵�����λ�������ɾ�����
            spawner.SpawnExperienceOrbs(transform.position);
        }

        // ���յ��˵�����أ�����������
        if (enemyPool != null)
        {
            enemyPool.ReturnObject(gameObject);  // �����˷��ص������
        }
        else
        {
            Debug.LogError("No ObjectPool found!");
            Destroy(gameObject);  // ����Ҳ�������أ���Ȼ���ٵ���
        }
    }

    // �ڸ��õ���ʱ��ʼ�����˵�����ֵ
    public void Initialize(int maxHp)
    {
        Hp = maxHp;
    }
}
