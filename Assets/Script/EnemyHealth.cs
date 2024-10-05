using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int Hp;
    public ExperienceOrbSpawner spawner;  // 经验球生成器
    public ObjectPool enemyPool;  // 引用对象池
    public string PoolTag;

    private void Start()
    {
        GameObject poolObject = GameObject.FindWithTag(PoolTag);
        if (poolObject != null)
        {
            enemyPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    // 敌人受伤
    public void TakeDamage(int damage, bool isBoom)
    {
        Hp -= damage;
        if (Hp <= 0)
        {
            Die(isBoom);
        }
    }

    // 敌人死亡
    void Die(bool isBoom)
    {
        // 调用经验球生成器
        spawner = GetComponent<ExperienceOrbSpawner>();
        if (spawner != null && !isBoom)
        {
            // 传入敌人的死亡位置来生成经验球
            spawner.SpawnExperienceOrbs(transform.position);
        }

        // 回收敌人到对象池，而不是销毁
        if (enemyPool != null)
        {
            enemyPool.ReturnObject(gameObject);  // 将敌人返回到对象池
        }
        else
        {
            Debug.LogError("No ObjectPool found!");
            Destroy(gameObject);  // 如果找不到对象池，仍然销毁敌人
        }
    }

    // 在复用敌人时初始化敌人的生命值
    public void Initialize(int maxHp)
    {
        Hp = maxHp;
    }
}
