using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class RangedEnemy : MonoBehaviour
{
    public Transform player;  // 玩家对象的Transform
    public GameObject bulletPrefab;  // 子弹预制体
    public float speed;  // 敌人移动速度
    public float stopDistance;  // 靠近到此距离时停止并攻击玩家 (a)
    public float chaseDistance;  // 玩家与敌人之间大于此距离时，继续靠近玩家 (b)
    public float fireRate;  // 子弹发射频率
    public float bulletSpeed;  // 子弹速度

    private Animator animator;  // 动画控制器
    private float nextFireTime = 0f;  // 下次发射子弹的时间

    private CharacterAtribute Character;//
    private EnemyHealth enemyHealth;

    void Start()
    {
        // 如果没有手动设置玩家对象，自动查找
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");  // 请确保 Tag 为 "player"
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }

        // 获取敌人的动画组件
        animator = GetComponent<Animator>();

        //获得健康组件
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        // 计算敌人与玩家的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 如果距离大于追击距离 b，继续移动靠近玩家
        if (distanceToPlayer > chaseDistance)
        {
            MoveTowardsPlayer();
            animator.SetBool("Attack", false);  // 取消攻击动画
        }
        // 如果距离在停止攻击距离 a 和 b 之间，停止并射击
        else if (distanceToPlayer <= stopDistance)
        {
            StopAndShoot();
        }
        else
        {
            MoveTowardsPlayer();
            animator.SetBool("Attack", false);  // 取消攻击动画
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

    // 停止并射击玩家
    void StopAndShoot()
    {
        animator.SetBool("Attack", true);  // 播放攻击动画

        // 敌人停止移动
        transform.Translate(Vector3.zero);

        // 如果到了发射子弹的时间
        if (Time.time >= nextFireTime)
        {
            // 发射子弹
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;  // 设置下一次发射时间
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Bullet"))
        {
            
            // 如果碰到子弹，敌人受伤
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Character.Atk,false);  // 调用敌人受伤函数并减少10点生命值
            }

            // 销毁子弹
            Destroy(collision.gameObject);
        }
    }

    // 发射子弹
    void FireBullet()
    {
        // 计算子弹方向
        Vector3 direction = (player.position - transform.position).normalized;

        // 生成子弹并设置位置和旋转
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // 获取子弹的 Rigidbody2D 并设置其速度
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        // 根据方向旋转子弹
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
