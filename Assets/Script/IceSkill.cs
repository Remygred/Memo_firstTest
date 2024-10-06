using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : MonoBehaviour
{
    public float freezeRadius = 2f;  // 冻结的半径范围
    public float freezeDuration = 3f;  // 冻结持续时间
    public ObjectPool iceEffectPool;  // 冰块效果的对象池
    public GameObject freezeIndicator;  // 蓝色范围标示
    public GameObject particlePrefab;

    public void IceSkillCall()
    {
        // 显示法术范围标示
        if (freezeIndicator != null)
        {
            GameObject indicator = Instantiate(freezeIndicator, transform.position, Quaternion.identity);
            indicator.transform.localScale = new Vector3(freezeRadius * 2, freezeRadius * 2, 1);
            indicator.transform.SetParent(this.transform);
            StartCoroutine(RemoveIndicatorAfterFreeze(indicator));
        }

        // 开始生成粒子
        StartCoroutine(GenerateParticles());

        // 在法术持续时间内持续监控敌人并冻结
        StartCoroutine(CheckAndFreezeEnemies());
    }

    IEnumerator CheckAndFreezeEnemies()
    {
        float elapsedTime = 0f;

        // 持续监控敌人直到法术结束
        while (elapsedTime < freezeDuration)
        {
            // 找到范围内的所有敌人
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, freezeRadius);

            foreach (Collider2D enemy in enemies)
            {
                if (enemy.CompareTag("Enemy") || enemy.CompareTag("REnemy"))
                {
                    EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();

                    if (enemyControl != null && !enemyControl.isFrozen)
                    {
                        StartCoroutine(FreezeEnemy(enemyControl, enemy.transform));
                    }
                }
            }

            elapsedTime += 0.2f;  // 每隔0.2秒检查一次
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FreezeEnemy(EnemyControl enemy, Transform enemyTransform)
    {
        // 生成冰块效果并附加到敌人身上
        GameObject freezeEffect = iceEffectPool.GetObject();
        freezeEffect.transform.position = enemyTransform.position;
        freezeEffect.transform.SetParent(enemyTransform);  // 使冰块效果跟随敌人

        // 冻结敌人
        enemy.Freeze(freezeDuration);

        // 获取敌人的健康组件
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

        // 持续监控敌人的状态
        float elapsedTime = 0f;
        while (elapsedTime < freezeDuration)
        {
            if (enemyHealth != null && enemyHealth.isDead)
            {
                // 如果敌人死亡，立刻归还冰块效果
                iceEffectPool.ReturnObject(freezeEffect);
                yield break;  // 退出协程
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 如果敌人没有死亡，冻结结束后，将冰块效果归还对象池
        iceEffectPool.ReturnObject(freezeEffect);
    }

    // 随机生成粒子
    IEnumerator GenerateParticles()
    {
        float elapsed = 0f;
        while (elapsed < freezeDuration)
        {
            // 在范围内生成粒子
            Vector3 randomPosition = transform.position + (Vector3)Random.insideUnitCircle * freezeRadius;
            GameObject particle = Instantiate(particlePrefab, randomPosition, Quaternion.identity);

            // 让粒子向上漂浮并逐渐消失
            StartCoroutine(AnimateParticle(particle));

            // 控制生成速度
            yield return new WaitForSeconds(0.1f);  // 控制粒子生成频率
            elapsed += 0.1f;
        }
    }

    // 粒子向上飘的动画
    IEnumerator AnimateParticle(GameObject particle)
    {
        float particleLifetime = 2f;  // 粒子的持续时间
        float elapsed = 0f;
        Vector3 startPosition = particle.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 2f;  // 向上漂浮

        SpriteRenderer renderer = particle.GetComponent<SpriteRenderer>();
        Color startColor = renderer.color;

        while (elapsed < particleLifetime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / particleLifetime;

            // 移动粒子向上
            particle.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // 让粒子逐渐消失
            renderer.color = Color.Lerp(startColor, Color.clear, t);

            yield return null;
        }

        // 粒子消失后销毁
        Destroy(particle);
    }

    // 冰冻结束后移除作用范围标示
    IEnumerator RemoveIndicatorAfterFreeze(GameObject indicator)
    {
        yield return new WaitForSeconds(freezeDuration);
        Destroy(indicator);  // 冻结结束后销毁标示
    }
}
