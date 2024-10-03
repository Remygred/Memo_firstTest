using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomer : MonoBehaviour
{
    public Transform player;  // ��Ҷ����Transform
    public float speed;  // �����ƶ��ٶ�
    public float explosionRadius;  // ��ը���˺���Χ
    public float detonationDistance;  // ������ҵ��Ա���������
    public int Atk;  // ��ը��ɵ��˺�
    public int Hp;

    private bool isExploding = false;  // �Ƿ����ڱ�ը
    private Animator animator;  // ����������

    private CharacterAtribute Character;
    private EnemyHealth enemyHealth;  // ���õ��˵Ľ����������

    void Start()
    {
        // �Զ����ҳ����е����
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }

        // ��ȡ���˵Ķ������
        animator = GetComponent<Animator>();

        // ��ȡ���˵Ľ����������
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        // ����Ѿ���ը���Ͳ����ƶ�
        if (isExploding) return;

        // �����������ҵľ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �������С�ڻ���ڱ�ը���룬������ը
        if (distanceToPlayer <= detonationDistance)
        {
            StartCoroutine(Detonate());  // ��ʼ��ըЭ��
        }
        else
        {
            MoveTowardsPlayer();  // ��������ҿ���
        }
    }

    // �ƶ���������ҿ���
    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        // �������λ�÷�ת���˳���
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);  // ����
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);  // ����
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

       if (collision.gameObject.CompareTag("Bullet"))
       {
            // ��������ӵ�����������
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Character.Atk,false);  // ���õ������˺�������������ֵ
            }

            // �����ӵ�
            Destroy(collision.gameObject);
       }
    }

    // ��ըЭ��
    IEnumerator Detonate()
    {
        isExploding = true;  // ���Ϊ���ڱ�ը
        animator.SetTrigger("Boom");  // ���ű�ը����

        // �ȴ��������ţ����趯������ʱ��Ϊ 1 �룬���Ը��ݶ�������ʱ�䣩
        yield return new WaitForSeconds(1f);

        // ��ⱬը��Χ�ڵ����ж���
        Collider2D[] objectsInRange = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D obj in objectsInRange)
        {
            // ���������˺�
            if (obj.CompareTag("player") && !Character.IsGetAttack)
            { 
                Character.TakeDamage(Atk);
            }

            // ��������������˺�
            if (obj.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(Atk,true);  // �õ����ܵ��˺�
                }
            }
        }

        // ��ը�������Ա���
        Destroy(gameObject);
    }
}
