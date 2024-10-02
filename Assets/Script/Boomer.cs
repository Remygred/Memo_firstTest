using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomer : MonoBehaviour
{
    public Transform player;  // 玩家对象的Transform
    public float speed = 2f;  // 敌人移动速度
    public float explosionRadius = 3f;  // 爆炸的伤害范围
    public float detonationDistance = 1f;  // 靠近玩家的自爆触发距离
    public int explosionDamage = 20;  // 爆炸造成的伤害

    private bool isExploding = false;  // 是否正在爆炸
    private Animator animator;  // 动画控制器

    void Start()
    {
        // 自动查找场景中的玩家
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
        // 如果已经爆炸，就不再移动
        if (isExploding) return;

        // 计算敌人与玩家的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 如果距离小于或等于爆炸距离，触发爆炸
        if (distanceToPlayer <= detonationDistance)
        {
            StartCoroutine(Detonate());  // 开始爆炸协程
        }
        else
        {
            MoveTowardsPlayer();  // 继续向玩家靠近
        }
    }

    // 移动敌人向玩家靠近
    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // 根据玩家位置翻转敌人朝向
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);  // 朝右
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);  // 朝左
        }
    }

    // 爆炸协程
    IEnumerator Detonate()
    {
        isExploding = true;  // 标记为正在爆炸
        animator.SetTrigger("Boom");  // 播放爆炸动画

        // 等待动画播放（假设动画播放时间为 1 秒，可以根据动画调整时间）
        yield return new WaitForSeconds(1f);

        // 检测爆炸范围内的所有对象
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D obj in objectsInRange)
        {
            // 对玩家造成伤害
            if (obj.CompareTag("player"))
            {
            }

            // 对其他敌人造成伤害
            if (obj.CompareTag("Enemy"))
            {
            }
        }

        // 爆炸后销毁自爆怪
        Destroy(gameObject);
    }
}
