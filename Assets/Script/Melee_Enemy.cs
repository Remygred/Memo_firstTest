using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_Enemy : MonoBehaviour
{
    public Transform player;  // ��Ҷ����Transform
    public float speed;  // �����ƶ��ٶ�
    private Animator animator;  // ����������

    private bool isAttacking = false;  // �Ƿ����ڹ���

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
        // ��ȡ���˵Ķ������
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ���û�й�����������ƶ������λ��
        if (!isAttacking)
        {
            MoveTowardsPlayer();
        }
    }

    // ʹ�� Transform �ƶ����˿������
    void MoveTowardsPlayer()
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

    // �������������ײ��Ĵ����¼�
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            // ����⵽���ʱֹͣ�ƶ�����ʼ����
            isAttacking = true;
            animator.SetBool("Attack",true);
        }
    }

    // �������뿪�����ײ��ʱ�ָ��ƶ�
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            isAttacking = false;  // �ָ��ƶ�
            animator.SetBool("Attack",false);
        }
    }
}
