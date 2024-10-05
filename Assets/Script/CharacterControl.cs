using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed ;
    float Move_x;
    float Move_y;
    Animator ani;

    public Gun gunScript;  // 关联枪的脚本
    public ObjectPool bulletPool;  // 引用子弹的对象池

    public AudioSource SkillaudioSource;
    public AudioClip SkillSound;

    public AudioSource Walk;
    public AudioClip WalkSound;

    private float curtime = 0f;
    private float settime = 0.3f;

    public void Start()
    {
        ani = GetComponent<Animator>();

        // 获取子弹对象池
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

        curtime += Time.deltaTime;

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
        if ((Move_x != 0f || Move_y != 0f) && curtime >= settime) 
        { 
            Walk.PlayOneShot(WalkSound);
            curtime = 0f;
        }
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

        // 计算每颗子弹之间的角度
        float angleStep = 360f / gunScript.currentAmmo;  // 每颗子弹之间的角度均匀分布
        float startAngle = 0f;  // 从0度开始发射

        for (int i = 0; i < gunScript.currentAmmo; i++)
        {
            // 计算旋转的角度
            float angle = startAngle + (angleStep * i);

            // 旋转角度来计算发射方向
            Vector3 direction = Quaternion.Euler(0, 0, angle) * Vector3.right;

            // 从对象池中获取子弹，而不是实例化
            GameObject bullet = bulletPool.GetObject();

            if (bullet != null)
            {
                // 设置子弹的位置和方向
                bullet.transform.position = transform.position;

                // 计算子弹的旋转角度，使其-x方向对准发射方向
                float bulletAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle + 180);

                // 获取子弹的 Rigidbody2D 来控制子弹运动
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                // 设定子弹的速度
                float bulletSpeed = gunScript.bulletSpeed;  // 使用枪的子弹速度
                rb.velocity = direction * bulletSpeed;

                // 激活子弹
                bullet.SetActive(true);
            }
        }

        gunScript.currentAmmo = 0;

        // 将子弹数清空，进入换弹状态
        gunScript.StartCoroutine(gunScript.Reload());
    }
}
