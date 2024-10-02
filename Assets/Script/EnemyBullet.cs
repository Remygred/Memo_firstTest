using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // �ӵ����������ڣ�����ʱ��
    public int damage = 10;  // �ӵ���ɵ��˺�

    void Start()
    {
        // ��ָ��ʱ��������ӵ�
        Destroy(gameObject, lifeTime);
    }

    // ���ӵ�����������ײ��ʱ
    void OnTriggerEnter2D(Collider2D other)
    {
        // ����������
        if (other.gameObject.CompareTag("player"))
        {
            // �����ӵ�
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // ����������ˣ������ӵ�
            Destroy(gameObject);
        }
    }
}
