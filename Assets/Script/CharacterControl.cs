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
    public float spreadAngle = 30f;  // ���ܷ���ĽǶȷ�Χ

    public void Start()
    {
        ani = GetComponent<Animator>();     
    }

    public void Update()
    {
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

        Vector3 Move = new Vector3(Move_x,Move_y,0);
        transform.Translate(Move * speed * Time.deltaTime);
    }

    void UseSkill()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // �����ǹ�ڵ����ķ���
        Vector3 direction = (mousePosition - gunScript.transform.position).normalized;

        // ��������ʣ���ӵ�������һ���ĽǶȷ�Χ��
        float angleStep = spreadAngle / gunScript.currentAmmo;
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < gunScript.currentAmmo; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 rotatedDirection = Quaternion.Euler(0, 0, angle) * direction;

            // ����ǹ�Ľű������ӵ�
            gunScript.Shoot(rotatedDirection);
        }

        gunScript.currentAmmo = 0;

        // ���ӵ�����գ����뻻��״̬
        gunScript.StartCoroutine(gunScript.Reload());
    }
}
