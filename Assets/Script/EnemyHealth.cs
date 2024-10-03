using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int Hp;
    public ExperienceOrbSpawner spawner;  

    //敌人受伤
    public void TakeDamage(int damage,bool isBoom)
    {
        Hp -= damage;
        if(Hp <= 0)
        {
            Die(isBoom);
        }
    }

    //敌人死亡
    void Die(bool isBoom)
    {
        // 调用经验球生成器
        spawner =GetComponent<ExperienceOrbSpawner>();
        if (spawner != null && !isBoom)
        {
            // 传入敌人的死亡位置来生成经验球
            spawner.SpawnExperienceOrbs(transform.position);
        }

        Destroy(gameObject);
    }
}
