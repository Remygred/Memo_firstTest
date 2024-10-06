using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy : MonoBehaviour
{
    public Transform player;  // ��Ҷ����Transform
    public float speed;  // �����ƶ��ٶ�
    public Animator animator;  // ����������
    public int Atk;  // ���˹�����

    private CharacterAtribute Character;

    private bool isAttacking = false;  // �Ƿ����ڹ���

    private EnemyHealth enemyHealth;  // ���õ��˵Ľ����������

    public AudioSource AtkaudioSource;
    public AudioClip AtkSound;

    public EnemyControl enemyControl;

    void Start()
    {
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");  // ��ȷ�� Tag Ϊ "player"
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        };

        // ��ȡ���˵Ľ����������
        enemyHealth = GetComponent<EnemyHealth>();
    }

    void Update()
    {
        if (enemyControl.isFrozen) return;

        // ���û�й�����������ƶ������λ��
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    private void OnEnable()
    {
        animator.SetBool("Attack", false);
    }

    // ʹ�� Transform �ƶ����˿������
    void MoveTowardsPlayer()
    {
        if (player != null)
        {
            // ���㷽��
            Vector3 direction = (player.position - transform.position).normalized;

            // ʹ�� Transform �ƶ�����
            transform.Translate(direction * speed * Time.deltaTime);

            // ������ҵ�λ�÷�ת���˳���
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);  // ����
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);  // ����
            }
        }
    }

    // ����������һ�������ӵ���ײ��Ĵ����¼�
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enemyControl.isFrozen && collision.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            // ����⵽���ʱֹͣ�ƶ�����ʼ����
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
            // ��������ӵ�����������
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(Character.Atk,false);  // ���õ������˺�������������ֵ
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

    // �������뿪�����ײ��ʱ�ָ��ƶ�
    void OnTriggerExit2D(Collider2D collision)
    {
        if (enemyControl.isFrozen) return;

        if (collision.gameObject.CompareTag("player"))
        {
            isAttacking = false;  // �ָ��ƶ�
            animator.SetBool("Attack", false);
        }
    }
}
