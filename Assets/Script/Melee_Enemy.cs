using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy : MonoBehaviour
{
    public Transform player;  // 玩家对象的Transform
    public float speed;  // 敌人移动速度
    private Animator animator;  // 动画控制器

    private bool isAttacking = false;  // 是否正在攻击

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        // 获取敌人的动画组件
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 如果没有攻击，则持续移动到玩家位置
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    // 使用 Transform 移动敌人靠近玩家
    void MoveTowardsPlayer()
    {
        // 计算方向
        Vector3 direction = (player.position - transform.position).normalized;

        // 使用 Transform 移动敌人
        transform.Translate(direction * speed * Time.deltaTime);    

        // 根据玩家的位置翻转敌人朝向
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);  // 朝右
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);  // 朝左
        }
    }

    // 检测敌人与玩家碰撞箱的触发事件
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            // 当检测到玩家时停止移动并开始攻击
            isAttacking = true;
            animator.SetBool("Attack",true);
        }
    }

    // 当敌人离开玩家碰撞箱时恢复移动
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            isAttacking = false;  // 恢复移动
            animator.SetBool("Attack",false);
        }
    }
}
