using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkill : MonoBehaviour
{
    public float duration = 5f;  // 闪电效果持续时间
    public float damagePerSecond = 10f;  // 每秒造成的伤害
    public float effectRadius = 10f;  // 闪电技能的作用范围

    public ObjectPool lightningPool;  // 引用闪电特效的对象池
    private List<GameObject> affectedEnemies;  // 受到闪电影响的敌人列表
    private List<GameObject> activeLightningEffects;  // 激活的闪电特效列表

    private Transform player;  // 角色的 Transform

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;

        // 初始化敌人列表和特效列表
        affectedEnemies = new List<GameObject>();
        activeLightningEffects = new List<GameObject>();

        // 检查对象池是否存在
        if (lightningPool == null)
        {
            Debug.LogError("Lightning Pool is not assigned!");
            return;
        }
    }

    // 启动闪电技能
    public void ActivateLightningSkill()
    {
        // 查找范围内的敌人
        FindEnemiesInRange();
        // 开始闪电效果
        StartCoroutine(ApplyLightningEffect());
    }

    void FindEnemiesInRange()
    {
        affectedEnemies.Clear();  // 清空之前的敌人列表

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject[] rEnemies = GameObject.FindGameObjectsWithTag("REnemy");

        foreach (GameObject enemy in enemies)
        {
            if (IsEnemyInRange(enemy.transform.position))
            {
                affectedEnemies.Add(enemy);
            }
        }

        foreach (GameObject enemy in rEnemies)
        {
            if (IsEnemyInRange(enemy.transform.position))
            {
                affectedEnemies.Add(enemy);
            }
        }
    }

    IEnumerator ApplyLightningEffect()
    {
        float elapsed = 0f;

        // 为每个敌人附加闪电特效
        foreach (GameObject enemy in affectedEnemies)
        {
            GameObject lightningEffect = lightningPool.GetObject();
            // 将闪电的下端对齐到敌人的位置，假设闪电的起点是上方
            lightningEffect.transform.position = new Vector3(
                enemy.transform.position.x,  // 保持与敌人的X轴对齐
                enemy.transform.position.y + lightningEffect.GetComponent<Renderer>().bounds.size.y / 2,  // 使闪电的下端对齐到敌人位置
                enemy.transform.position.z
            );
            lightningEffect.transform.SetParent(enemy.transform);  // 让闪电特效跟随敌人
            activeLightningEffects.Add(lightningEffect);  // 将闪电特效存储到列表中
        }

        // 在效果持续时间内对敌人持续造成伤害
        while (elapsed < duration)
        {
            foreach (GameObject enemy in affectedEnemies)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerSecond * Time.deltaTime, false);  // 对敌人造成持续伤害
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // 效果结束后，回收所有的闪电特效
        foreach (var lightningEffect in activeLightningEffects)
        {
            lightningPool.ReturnObject(lightningEffect);  // 将闪电特效归还到对象池
        }
        activeLightningEffects.Clear();  // 清空激活列表
    }

    // 判断敌人是否在角色周围的范围内
    bool IsEnemyInRange(Vector3 enemyPos)
    {
        float distance = Vector3.Distance(player.position, enemyPos);
        return distance <= effectRadius;  // 判断敌人与角色的距离是否在作用范围内
    }

}
