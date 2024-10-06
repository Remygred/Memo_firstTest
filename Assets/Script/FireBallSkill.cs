using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : MonoBehaviour
{
    public float fireballSpeed = 10f;  // 火球速度
    public float explosionRadius = 2f; // 爆炸范围
    public float fireballDamage = 50;    // 火球伤害
    public float fireballLifetime = 5f; // 火球的生命周期

    private Animator ani;
    private bool hasExploded = false; // 防止多次爆炸
    private Rigidbody2D rb;

    // 发射火球
    public void CastFireball(Vector3 targetPosition)
    {
        // 计算火球飞行的方向
        Vector3 direction = (targetPosition - transform.position).normalized;

        // 赋予火球初始速度
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireballSpeed;

        // 启动火球生命周期协程
        StartCoroutine(HandleFireballLifetime());
    }

    // 控制火球的生命周期
    IEnumerator HandleFireballLifetime()
    {
        float timeElapsed = 0f;

        // 在规定时间内飞行
        while (timeElapsed < fireballLifetime && !hasExploded)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // 如果火球还未爆炸，达到生命周期时爆炸
        if (!hasExploded)
        {
            Explode();
        }
    }

    // 火球爆炸逻辑
    void Explode()
    {
        if (hasExploded) return;  // 防止多次触发爆炸

        hasExploded = true; // 标记为已爆炸

        rb.velocity = Vector3.zero; 

        ani = GetComponent<Animator>();
        if (ani != null)
        {
            ani.SetTrigger("boom");  // 播放爆炸动画
        }

        // 获取爆炸范围内的所有敌人
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("REnemy"))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(fireballDamage, false);  // 对敌人造成伤害
                }
            }
        }

        // 等待爆炸动画播放完毕再销毁火球
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // 等待爆炸动画的完成时间
        if (ani != null)
        {
            yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(gameObject);
    }

    // 火球碰到敌人时爆炸
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("REnemy")) && !hasExploded)
        {
            // 碰到敌人时爆炸
            Explode();
        }
    }

    public void IncreaseExplosionDamage()
    {
        fireballDamage *= 1.2f;  // 增加火球伤害
    }

    public void IncreaseExplosionRadius()
    {
        explosionRadius *= 1.2f;  // 扩大火球爆炸范围
        fireballDamage *= 0.95f;
    }

}
