using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    public Transform player;  // ��Ҷ����Transform
    public GameObject bulletPrefab;  // �ӵ�Ԥ����
    public float speed;  // �����ƶ��ٶ�
    public float stopDistance;  // �������˾���ʱֹͣ��������� (a)
    public float chaseDistance;  // ��������֮����ڴ˾���ʱ������������� (b)
    public float fireRate;  // �ӵ�����Ƶ��
    public float bulletSpeed;  // �ӵ��ٶ�

    private Animator animator;  // ����������
    private bool isShooting = false;  // �Ƿ����ڹ���
    private float nextFireTime = 0f;  // �´η����ӵ���ʱ��

    void Start()
    {
        // ���û���ֶ�������Ҷ����Զ�����
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
        // �����������ҵľ���
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // ����������׷������ b�������ƶ��������
        if (distanceToPlayer > chaseDistance)
        {
            MoveTowardsPlayer();
            isShooting = false;
            animator.SetBool("Attack", false);  // ȡ����������
        }
        // ���������ֹͣ�������� a �� b ֮�䣬ֹͣ�����
        else if (distanceToPlayer <= stopDistance)
        {
            StopAndShoot();
        }
        else
        {
            MoveTowardsPlayer();
            isShooting = false;
            animator.SetBool("Attack", false);  // ȡ����������
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

    // ֹͣ��������
    void StopAndShoot()
    {
        isShooting = true;
        animator.SetBool("Attack", true);  // ���Ź�������

        // ����ֹͣ�ƶ�
        transform.Translate(Vector3.zero);

        // ������˷����ӵ���ʱ��
        if (Time.time >= nextFireTime)
        {
            // �����ӵ�
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;  // ������һ�η���ʱ��
        }
    }

    // �����ӵ�
    void FireBullet()
    {
        // �����ӵ�����
        Vector3 direction = (player.position - transform.position).normalized;

        // �����ӵ�������λ�ú���ת
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // ��ȡ�ӵ��� Rigidbody2D ���������ٶ�
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = direction * bulletSpeed;

        // ���ݷ�����ת�ӵ�
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
