using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // 子弹的生命周期，飞行时间
    public int Atk;  // 子弹造成的伤害
    private CharacterAtribute Character;
    private ObjectPool bulletPool;  // 引用对象池

    void Start()
    {
        // 查找对象池，假设你已经有带有"BulletPool"标签的对象池
        GameObject poolObject = GameObject.FindWithTag("EnemyBulletPool");
        if (poolObject != null)
        {
            bulletPool = poolObject.GetComponent<ObjectPool>();
        }

        // 如果玩家对象未设置，自动查找
        if (Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }

        // 启动生命周期倒计时
        StartCoroutine(ReturnToPoolAfterTime());
    }

    // 使用协程在一定时间后将子弹返回对象池
    IEnumerator ReturnToPoolAfterTime()
    {
        yield return new WaitForSeconds(lifeTime);
        ReturnToPool();
    }

    // 当子弹碰到其他碰撞体时
    void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到玩家
        if (other.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            // 让玩家受到伤害
            Character.TakeDamage(Atk);

            // 将子弹返回对象池
            ReturnToPool();
        }
        else if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("player") && Character.IsGetAttack)
        {
            ReturnToPool();  // 将子弹返回对象池
        }
    }

    // 将子弹返回对象池而不是销毁
    void ReturnToPool()
    {
        if (bulletPool != null)
        {
            bulletPool.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogError("对象池未找到！");
            Destroy(gameObject); 
        }
    }
}
