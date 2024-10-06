using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSkill : MonoBehaviour
{
    public float freezeRadius = 2f;  // ����İ뾶��Χ
    public float freezeDuration = 3f;  // �������ʱ��
    public ObjectPool iceEffectPool;  // ����Ч���Ķ����
    public GameObject freezeIndicator;  // ��ɫ��Χ��ʾ
    public GameObject particlePrefab;

    public void IceSkillCall()
    {
        // ��ʾ������Χ��ʾ
        if (freezeIndicator != null)
        {
            GameObject indicator = Instantiate(freezeIndicator, transform.position, Quaternion.identity);
            indicator.transform.localScale = new Vector3(freezeRadius * 2, freezeRadius * 2, 1);
            indicator.transform.SetParent(this.transform);
            StartCoroutine(RemoveIndicatorAfterFreeze(indicator));
        }

        // ��ʼ��������
        StartCoroutine(GenerateParticles());

        // �ڷ�������ʱ���ڳ�����ص��˲�����
        StartCoroutine(CheckAndFreezeEnemies());
    }

    IEnumerator CheckAndFreezeEnemies()
    {
        float elapsedTime = 0f;

        // ������ص���ֱ����������
        while (elapsedTime < freezeDuration)
        {
            // �ҵ���Χ�ڵ����е���
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

            elapsedTime += 0.2f;  // ÿ��0.2����һ��
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator FreezeEnemy(EnemyControl enemy, Transform enemyTransform)
    {
        // ���ɱ���Ч�������ӵ���������
        GameObject freezeEffect = iceEffectPool.GetObject();
        freezeEffect.transform.position = enemyTransform.position;
        freezeEffect.transform.SetParent(enemyTransform);  // ʹ����Ч���������

        // �������
        enemy.Freeze(freezeDuration);

        // ��ȡ���˵Ľ������
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();

        // ������ص��˵�״̬
        float elapsedTime = 0f;
        while (elapsedTime < freezeDuration)
        {
            if (enemyHealth != null && enemyHealth.isDead)
            {
                // ����������������̹黹����Ч��
                iceEffectPool.ReturnObject(freezeEffect);
                yield break;  // �˳�Э��
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �������û����������������󣬽�����Ч���黹�����
        iceEffectPool.ReturnObject(freezeEffect);
    }

    // �����������
    IEnumerator GenerateParticles()
    {
        float elapsed = 0f;
        while (elapsed < freezeDuration)
        {
            // �ڷ�Χ����������
            Vector3 randomPosition = transform.position + (Vector3)Random.insideUnitCircle * freezeRadius;
            GameObject particle = Instantiate(particlePrefab, randomPosition, Quaternion.identity);

            // ����������Ư��������ʧ
            StartCoroutine(AnimateParticle(particle));

            // ���������ٶ�
            yield return new WaitForSeconds(0.1f);  // ������������Ƶ��
            elapsed += 0.1f;
        }
    }

    // ��������Ʈ�Ķ���
    IEnumerator AnimateParticle(GameObject particle)
    {
        float particleLifetime = 2f;  // ���ӵĳ���ʱ��
        float elapsed = 0f;
        Vector3 startPosition = particle.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * 2f;  // ����Ư��

        SpriteRenderer renderer = particle.GetComponent<SpriteRenderer>();
        Color startColor = renderer.color;

        while (elapsed < particleLifetime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / particleLifetime;

            // �ƶ���������
            particle.transform.position = Vector3.Lerp(startPosition, endPosition, t);

            // ����������ʧ
            renderer.color = Color.Lerp(startColor, Color.clear, t);

            yield return null;
        }

        // ������ʧ������
        Destroy(particle);
    }

    // �����������Ƴ����÷�Χ��ʾ
    IEnumerator RemoveIndicatorAfterFreeze(GameObject indicator)
    {
        yield return new WaitForSeconds(freezeDuration);
        Destroy(indicator);  // ������������ٱ�ʾ
    }
}
