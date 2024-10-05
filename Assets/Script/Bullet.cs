using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bullet : MonoBehaviour
{
    private Renderer bulletRenderer;
    private ObjectPool bulletPool;  // ���������
    public string poolTag = "BulletPool";

    void Start()
    {
        bulletRenderer = GetComponent<Renderer>();

        // ���Ҵ����ض���ǩ�Ķ����
        GameObject poolObject = GameObject.FindWithTag(poolTag);
        if (poolObject != null)
        {
            bulletPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    void Update()
    {
        // ����ӵ����ٿɼ����黹�������
        if (!bulletRenderer.isVisible)
        {
            ReturnToPool();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("REnemy"))
        {
            ReturnToPool();
        }
    }

    void ReturnToPool()
    {
        // ���ö���صĻ��պ����������������ӵ�
        if (bulletPool != null)
        {
            bulletPool.ReturnObject(gameObject);  // ���ӵ����ص������
        }
        else
        {
            Debug.LogError("ObjectPool not found!");
        }
    }
}
