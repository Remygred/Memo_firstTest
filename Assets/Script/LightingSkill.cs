using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSkill : MonoBehaviour
{
    public float duration = 5f;  // ����Ч������ʱ��
    public float damagePerSecond = 10f;  // ÿ����ɵ��˺�
    public float effectRadius = 10f;  // ���缼�ܵ����÷�Χ

    public ObjectPool lightningPool;  // ����������Ч�Ķ����
    private List<GameObject> affectedEnemies;  // �ܵ�����Ӱ��ĵ����б�
    private List<GameObject> activeLightningEffects;  // �����������Ч�б�

    private Transform player;  // ��ɫ�� Transform

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("player").transform;

        // ��ʼ�������б����Ч�б�
        affectedEnemies = new List<GameObject>();
        activeLightningEffects = new List<GameObject>();

        // ��������Ƿ����
        if (lightningPool == null)
        {
            Debug.LogError("Lightning Pool is not assigned!");
            return;
        }
    }

    // �������缼��
    public void ActivateLightningSkill()
    {
        // ���ҷ�Χ�ڵĵ���
        FindEnemiesInRange();
        // ��ʼ����Ч��
        StartCoroutine(ApplyLightningEffect());
    }

    void FindEnemiesInRange()
    {
        affectedEnemies.Clear();  // ���֮ǰ�ĵ����б�

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

        // Ϊÿ�����˸���������Ч
        foreach (GameObject enemy in affectedEnemies)
        {
            GameObject lightningEffect = lightningPool.GetObject();
            // ��������¶˶��뵽���˵�λ�ã����������������Ϸ�
            lightningEffect.transform.position = new Vector3(
                enemy.transform.position.x,  // ��������˵�X�����
                enemy.transform.position.y + lightningEffect.GetComponent<Renderer>().bounds.size.y / 2,  // ʹ������¶˶��뵽����λ��
                enemy.transform.position.z
            );
            lightningEffect.transform.SetParent(enemy.transform);  // ��������Ч�������
            activeLightningEffects.Add(lightningEffect);  // ��������Ч�洢���б���
        }

        // ��Ч������ʱ���ڶԵ��˳�������˺�
        while (elapsed < duration)
        {
            foreach (GameObject enemy in affectedEnemies)
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damagePerSecond * Time.deltaTime, false);  // �Ե�����ɳ����˺�
                }
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        // Ч�������󣬻������е�������Ч
        foreach (var lightningEffect in activeLightningEffects)
        {
            lightningPool.ReturnObject(lightningEffect);  // ��������Ч�黹�������
        }
        activeLightningEffects.Clear();  // ��ռ����б�
    }

    // �жϵ����Ƿ��ڽ�ɫ��Χ�ķ�Χ��
    bool IsEnemyInRange(Vector3 enemyPos)
    {
        float distance = Vector3.Distance(player.position, enemyPos);
        return distance <= effectRadius;  // �жϵ������ɫ�ľ����Ƿ������÷�Χ��
    }

}
