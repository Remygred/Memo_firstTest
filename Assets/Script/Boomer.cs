using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomer : MonoBehaviour
{
    public Transform player;  // ��Ҷ����Transform
    public float speed = 2f;  // �����ƶ��ٶ�
    public float explosionRadius = 3f;  // ��ը���˺���Χ
    public float detonationDistance = 1f;  // ������ҵ��Ա���������
    public int explosionDamage = 20;  // ��ը��ɵ��˺�

    private bool isExploding = false;  // �Ƿ����ڱ�ը
    private Animator animator;  // ����������

    void Start()
    {
        // �Զ����ҳ����е����
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
            if (obj.CompareTag("player"))
            {
            }

            // ��������������˺�
            if (obj.CompareTag("Enemy"))
            {
            }
        }

        // ��ը�������Ա���
        Destroy(gameObject);
    }
}
