using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 5f;
    float Move_x;
    float Move_y;
    Animator ani;

    public Gun gunScript;  // ����ǹ�Ľű�
    public ObjectPool bulletPool;  // �����ӵ��Ķ����

    public AudioSource SkillaudioSource;
    public AudioClip SkillSound;

    public void Start()
    {
        ani = GetComponent<Animator>();

        // ��ȡ�ӵ������
        GameObject poolObject = GameObject.FindWithTag("BulletPool");
        if (poolObject != null)
        {
            bulletPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    public void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        MoveAnimation();
        Move();

        // ��������Ҽ��ļ��ܴ���
        if (Input.GetMouseButtonDown(1) && gunScript.currentAmmo > 0)
        {
            UseSkill();
        }
    }

    void MoveAnimation()
    {
        ani.SetBool("isMovingR", Move_x > 0);
        ani.SetBool("isMovingL", Move_x < 0);
    }

    void Move()
    {
        Move_x = Input.GetAxis("Horizontal");
        Move_y = Input.GetAxis("Vertical");

        Vector3 Move = new Vector3(Move_x, Move_y, 0);
        transform.Translate(Move * speed * Time.deltaTime);
    }

    void UseSkill()
    {
        if (SkillaudioSource != null && SkillSound != null)
        {
            SkillaudioSource.PlayOneShot(SkillSound);
        }

        // ����ÿ���ӵ�֮��ĽǶ�
        float angleStep = 360f / gunScript.currentAmmo;  // ÿ���ӵ�֮��ĽǶȾ��ȷֲ�
        float startAngle = 0f;  // ��0�ȿ�ʼ����

        for (int i = 0; i < gunScript.currentAmmo; i++)
        {
            // ������ת�ĽǶ�
            float angle = startAngle + (angleStep * i);

            // ��ת�Ƕ������㷢�䷽��
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;

            // �Ӷ�����л�ȡ�ӵ���������ʵ����
            GameObject bullet = bulletPool.GetObject();

            if (bullet != null)
            {
                // �����ӵ���λ�úͷ���
                bullet.transform.position = transform.position;

                // �����ӵ�����ת�Ƕȣ�ʹ��-x�����׼���䷽��
                float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 180);

                // ��ȡ�ӵ��� Rigidbody2D �������ӵ��˶�
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                // �趨�ӵ����ٶ�
                float bulletSpeed = gunScript.bulletSpeed;  // ʹ��ǹ���ӵ��ٶ�
                rb.velocity = direction * bulletSpeed;

                // �����ӵ�
                bullet.SetActive(true);
            }
        }

        gunScript.currentAmmo = 0;

        // ���ӵ�����գ����뻻��״̬
        gunScript.StartCoroutine(gunScript.Reload());
    }
}
