using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed = 5f;
    float Move_x;
    float Move_y;
    Animator ani;

    public Gun gunScript;  // 关联枪的脚本
    public float spreadAngle = 30f;  // 技能发射的角度范围

    public void Start()
    {
        ani = GetComponent<Animator>();     
    }

    public void Update()
    {
        MoveAnimation();
        Move();
        // 监听鼠标右键的技能触发
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

        // 计算从枪口到鼠标的方向
        Vector3 direction = (mousePosition - gunScript.transform.position).normalized;

        // 发射所有剩余子弹，并在一定的角度范围内
        float angleStep = spreadAngle / gunScript.currentAmmo;
        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < gunScript.currentAmmo; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector3 rotatedDirection = Quaternion.Euler(0, 0, angle) * direction;

            // 调用枪的脚本发射子弹
            gunScript.Shoot(rotatedDirection);
        }

        gunScript.currentAmmo = 0;

        // 将子弹数清空，进入换弹状态
        gunScript.StartCoroutine(gunScript.Reload());
    }
}
