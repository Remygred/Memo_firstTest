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
        // 子弹的预制体
        GameObject bulletPrefab = gunScript.bulletPrefab;  // 假设你已经在枪的脚本中有引用

        // 计算每颗子弹之间的角度
        float angleStep = 360f / gunScript.currentAmmo;  // 每颗子弹之间的角度均匀分布
        float startAngle = 0f;  // 从0度开始发射

        for (int i = 0; i < gunScript.currentAmmo; i++)
        {
            // 计算旋转的角度
            float angle = startAngle + (angleStep * i);

            // 旋转角度来计算发射方向
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;

            // 在角色位置实例化子弹
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

            // 计算子弹的旋转角度，使其-x方向对准发射方向
            float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 180);  // +180度使-x方向对准发射方向

            // 获取子弹的 Rigidbody2D 来控制子弹运动
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            // 设定子弹的速度
            float bulletSpeed = gunScript.bulletSpeed;  // 使用枪的子弹速度
            rb.velocity = direction * bulletSpeed;
        }

        gunScript.currentAmmo = 0;

        // 将子弹数清空，进入换弹状态
        gunScript.StartCoroutine(gunScript.Reload());
    }

}
