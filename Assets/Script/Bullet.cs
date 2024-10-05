using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class Bullet : MonoBehaviour
{
    private Renderer bulletRenderer;
    private ObjectPool bulletPool;  // 对象池引用
    public string poolTag = "BulletPool";

    void Start()
    {
        bulletRenderer = GetComponent<Renderer>();

        // 查找带有特定标签的对象池
        GameObject poolObject = GameObject.FindWithTag(poolTag);
        if (poolObject != null)
        {
            bulletPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    void Update()
    {
        // 如果子弹不再可见，归还到对象池
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
        // 调用对象池的回收函数，而不是销毁子弹
        if (bulletPool != null)
        {
            bulletPool.ReturnObject(gameObject);  // 将子弹返回到对象池
        }
        else
        {
            Debug.LogError("ObjectPool not found!");
        }
    }
}
