using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // �ӵ����������ڣ�����ʱ��
    public int Atk;  // �ӵ���ɵ��˺�
    private CharacterAtribute Character;
    private ObjectPool bulletPool;  // ���ö����

    void Start()
    {
        // ���Ҷ���أ��������Ѿ��д���"BulletPool"��ǩ�Ķ����
        GameObject poolObject = GameObject.FindWithTag("EnemyBulletPool");
        if (poolObject != null)
        {
            bulletPool = poolObject.GetComponent<ObjectPool>();
        }

        // �����Ҷ���δ���ã��Զ�����
        if (Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }

        // �����������ڵ���ʱ
        StartCoroutine(ReturnToPoolAfterTime());
    }

    // ʹ��Э����һ��ʱ����ӵ����ض����
    IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
    }

    // ���ӵ�����������ײ��ʱ
    void OnTriggerEnter2D(Collider2D other)
    {
        // ����������
        if (other.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            // ������ܵ��˺�
            Character.TakeDamage(Atk);

            // ���ӵ����ض����
            ReturnToPool();
        }
        else if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("player") && Character.IsGetAttack)
        {
            ReturnToPool();  // ���ӵ����ض����
        }
    }

    // ���ӵ����ض���ض���������
    void ReturnToPool()
    {
        if (bulletPool != null)
        {
            bulletPool.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogError("�����δ�ҵ���");
            Destroy(gameObject); 
        }
    }
}
