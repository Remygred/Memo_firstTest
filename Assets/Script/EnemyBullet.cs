using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // 子弹的生命周期，飞行时间
    public int damage = 10;  // 子弹造成的伤害

    void Start()
    {
        // 在指定时间后销毁子弹
        Destroy(gameObject, lifeTime);
    }

    // 当子弹碰到其他碰撞体时
    void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到玩家
        if (other.gameObject.CompareTag("player"))
        {
            // 销毁子弹
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            // 如果碰到敌人，销毁子弹
            Destroy(gameObject);
        }
    }
}
