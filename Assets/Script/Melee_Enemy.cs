using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy : MonoBehaviour
{
    public Transform player;  // 玩家对象的Transform
    public float speed;  // 敌人移动速度
    public Animator animator;  // 动画控制器
    public int Atk;  // 敌人攻击力

    private CharacterAtribute Character;

    private bool isAttacking = false;  // 是否正在攻击

    private EnemyHealth enemyHealth;  // 引用敌人的健康管理组件

    public AudioSource AtkaudioSource;
    public AudioClip AtkSound;

    public EnemyControl enemyControl;

    void Start()
    {
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");  // 请确保 Tag 为 "player"
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        };

        // 获取敌人的健康管理组件
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (enemyControl.isFrozen) return;

        // 如果没有攻击，则持续移动到玩家位置
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    private void OnEnable()
    {
        animator.SetBool("Attack", false);
    }

    // 使用 Transform 移动敌人靠近玩家
    void MoveTowardsPlayer()
    {
        if (player != null)
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
    }

    // 检测敌人与玩家或者玩家子弹碰撞箱的触发事件
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemyControl.isFrozen && collision.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            // 当检测到玩家时停止移动并开始攻击
            if (AtkaudioSource != null && AtkSound != null)
            {
                AtkaudioSource.PlayOneShot(AtkSound);
            }
            isAttacking = true;
            animator.SetBool("Attack", true);
            Character.TakeDamage(Atk);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            // 如果碰到子弹，敌人受伤
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Character.Atk,false);  // 调用敌人受伤函数并减少生命值
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (enemyControl.isFrozen) return;

        if (collision.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            if (AtkaudioSource != null && AtkSound != null)
            {
                AtkaudioSource.PlayOneShot(AtkSound);
            }
            isAttacking = true;
            animator.SetBool("Attack", true);
            Character.TakeDamage(Atk);
        }
    }

    // 当敌人离开玩家碰撞箱时恢复移动
    void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyControl.isFrozen) return;

        if (collision.gameObject.CompareTag("player"))
        {
            isAttacking = false;  // 恢复移动
            animator.SetBool("Attack", false);
        }
    }
}
