using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float Hp;
    public float oHp;
    public ExperienceOrbSpawner spawner;  // ������������
    public ObjectPool enemyPool;  // ���ö����
    public string PoolTag;
    public bool isDead = false;

    private void Start()
    {
        GameObject poolObject = GameObject.FindWithTag(PoolTag);
        if (poolObject != null)
        {
            enemyPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    private void OnEnable()
    {
        Hp = oHp;
        isDead = false;
    }

    // ��������
    public void TakeDamage(float damage, bool isBoom)
    {
        Hp -= damage;
        if (Hp <= 0 && !isDead)
        {
            Die(isBoom);
        }
    }

    // ��������
    void Die(bool isBoom)
    {
        isDead = true;
        // ���þ�����������
        spawner = GetComponent<ExperienceOrbSpawner>();
        if (spawner != null && !isBoom)
        {
            // ������˵�����λ�������ɾ�����
            spawner.SpawnExperienceOrbs(transform.position);
        }

        if (enemyPool != null)
        {
            enemyPool.ReturnObject(gameObject);  // �����˷��ص������
        }
    }

    // �ڸ��õ���ʱ��ʼ�����˵�����ֵ
    public void Initialize(int maxHp)
    {
        Hp = maxHp;
    }
}
