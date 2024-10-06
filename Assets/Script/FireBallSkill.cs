using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballSkill : MonoBehaviour
{
    public float fireballSpeed = 10f;  // �����ٶ�
    public float explosionRadius = 2f; // ��ը��Χ
    public float fireballDamage = 50;    // �����˺�
    public float fireballLifetime = 5f; // �������������

    private Animator ani;
    private bool hasExploded = false; // ��ֹ��α�ը
    private Rigidbody2D rb;

    // �������
    public void CastFireball(Vector3 targetPosition)
    {
        // ���������еķ���
        Vector3 direction = (targetPosition - transform.position).normalized;

        // ��������ʼ�ٶ�
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * fireballSpeed;

        // ����������������Э��
        StartCoroutine(HandleFireballLifetime());
    }

    // ���ƻ������������
    IEnumerator HandleFireballLifetime()
    {
        float timeElapsed = 0f;

        // �ڹ涨ʱ���ڷ���
        while (timeElapsed < fireballLifetime && !hasExploded)
        {
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // �������δ��ը���ﵽ��������ʱ��ը
        if (!hasExploded)
        {
            Explode();
        }
    }

    // ����ը�߼�
    void Explode()
    {
        if (hasExploded) return;  // ��ֹ��δ�����ը

        hasExploded = true; // ���Ϊ�ѱ�ը

        rb.velocity = Vector3.zero; 

        ani = GetComponent<Animator>();
        if (ani != null)
        {
            ani.SetTrigger("boom");  // ���ű�ը����
        }

        // ��ȡ��ը��Χ�ڵ����е���
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Enemy") || enemy.CompareTag("REnemy"))
            {
                EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(fireballDamage, false);  // �Ե�������˺�
                }
            }
        }

        // �ȴ���ը����������������ٻ���
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation()
    {
        // �ȴ���ը���������ʱ��
        if (ani != null)
        {
            yield return new WaitForSeconds(ani.GetCurrentAnimatorStateInfo(0).length);
        }
        Destroy(gameObject);
    }

    // ������������ʱ��ը
    void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.CompareTag("Enemy") || other.CompareTag("REnemy")) && !hasExploded)
        {
            // ��������ʱ��ը
            Explode();
        }
    }

    public void IncreaseExplosionDamage()
    {
        fireballDamage *= 1.2f;  // ���ӻ����˺�
    }

    public void IncreaseExplosionRadius()
    {
        explosionRadius *= 1.2f;  // �������ը��Χ
        fireballDamage *= 0.95f;
    }

}
