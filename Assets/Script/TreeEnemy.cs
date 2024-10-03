using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemy : MonoBehaviour
{
    public Transform player;  // ��Ҷ����Transform
    public float eyeOpenDistance;  // ��ҿ������˾���ʱ������
    public int contactDamage;  // ���������ʱ�ܵ����˺�
    private bool isEyeOpen = false;  // ���Ƿ�����
    private Animator animator;  // ����������

    public int Atk;
    private CharacterAtribute Character;
    void Start()
    {
        // ���û���ֶ�������Ҷ����Զ�����
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }

        // ��ȡ���Ķ������
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ������������ľ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // �����ҿ�����һ�����룬������
        if (distanceToPlayer <= eyeOpenDistance && !isEyeOpen)
        {
            OpenEyes();
        }
        // ���Զ�������ر��۾�
        else if (distanceToPlayer > eyeOpenDistance && isEyeOpen)
        {
            CloseEyes();
        }
    }

    // �����������ʱ����
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            animator.SetBool("Attack",true);
            Character.TakeDamage(Atk);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            animator.SetBool("Attack", false);
        }
    }

    // ���ӵ�������ʱ������ס�ӵ�
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet") || other.CompareTag("EnemyBullet"))
        {
            // �ӵ�����ס�������ӵ�
            Destroy(other.gameObject);
        }
    }

    // ������
    void OpenEyes()
    {
        isEyeOpen = true;
        animator.SetBool("Player_Close", true);  // �������۶���
    }

    // ���ر��۾�
    void CloseEyes()
    {
        isEyeOpen = false;
        animator.SetBool("Player_Close", false);  // ���ű��۶���
    }
}
