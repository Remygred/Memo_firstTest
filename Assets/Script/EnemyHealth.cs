using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public float Hp;
    public float oHp;
    public ExperienceOrbSpawner spawner;  // 经验球生成器
    public ObjectPool enemyPool;  // 引用对象池
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

    // 敌人受伤
    public void TakeDamage(float damage, bool isBoom)
    {
        Hp -= damage;
        if (Hp <= 0 && !isDead)
        {
            Die(isBoom);
        }
    }

    // 敌人死亡
    void Die(bool isBoom)
    {
        isDead = true;
        // 调用经验球生成器
        spawner = GetComponent<ExperienceOrbSpawner>();
        if (spawner != null && !isBoom)
        {
            // 传入敌人的死亡位置来生成经验球
            spawner.SpawnExperienceOrbs(transform.position);
        }

        if (enemyPool != null)
        {
            enemyPool.ReturnObject(gameObject);  // 将敌人返回到对象池
        }
    }

    // 在复用敌人时初始化敌人的生命值
    public void Initialize(int maxHp)
    {
        Hp = maxHp;
    }
}
