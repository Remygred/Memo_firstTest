using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // 子弹的生命周期，飞行时间
    public int Atk;  // 子弹造成的伤害
    private CharacterAtribute Character;

    void Start()
    {
        // 在指定时间后销毁子弹
        Destroy(gameObject, lifeTime);
        if (Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");  // 请确保 Tag 为 "player"
            if (playerObj != null)
            {
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }
    }

    // 当子弹碰到其他碰撞体时
    void OnTriggerEnter2D(Collider2D other)
    {
        // 如果碰到玩家
        if (other.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            // 销毁子弹
            Destroy(gameObject);
            Character.TakeDamage(Atk);
        }else if(other.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            Destroy(gameObject);
        }
    }
}
