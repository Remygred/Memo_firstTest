using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed;
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

    // 法术CD时间
    public float skillCooldown = 1.5f;
    public float lightningCooldown = 5f;
    public float fireballCooldown = 7f;
    public float iceCooldown = 10f;

    // 法术CD计时器
    public float skillTimer = 0f;
    public float lightningTimer = 0f;
    public float fireballTimer = 0f;
    public float iceTimer = 0f;

    // 火法术的状态标志
    public bool isPreparingFireball = false;
    public GameObject fireballPrefab;  // 火球预制体

    public LightningSkill lightningSkill;
    public FireballSkill fireballSkill;
    public IceSkill IceSkill;

    public AudioSource lightaudiosourse;
    public AudioSource fireballaudiosourse;
    public AudioSource iceaudiosource;

    public AudioClip lights;
    public AudioClip fireballs;
    public AudioClip ices;

    void Start()
    {
        ani = GetComponent<Animator>();

        // 获取子弹对象池
        GameObject poolObject = GameObject.FindWithTag("BulletPool");
        if (poolObject != null)
        {
            bulletPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        curtime += Time.deltaTime;

        // 更新法术CD计时器
        if(skillTimer > 0)  skillTimer -= Time.deltaTime; 
        if(lightningTimer > 0) lightningTimer -= Time.deltaTime;
        if(fireballTimer > 0) fireballTimer -= Time.deltaTime;
        if(iceTimer > 0) iceTimer -= Time.deltaTime;

        MoveAnimation();
        Move();

        // 监听鼠标右键的技能触发
        if (Input.GetMouseButtonDown(1) && skillTimer <= 0f)
        {
            UseSkill();
        }
        // 监听法术按键
        if (Input.GetKeyDown(KeyCode.Alpha1) && lightningTimer <= 0f)
        {
            CastLightning();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && fireballTimer <= 0f)
        {
            isPreparingFireball = true;  // 准备火法术
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && iceTimer <= 0f)
        {
            CastIce();
        }

        // 火法术在按键后点击鼠标左键才释放
        if (isPreparingFireball && Input.GetMouseButtonDown(0))
        {
            CastFireball();
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
        skillTimer = skillCooldown;
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

    // 雷法术：对所有敌人造成持续伤害
    void CastLightning()
    {
        lightaudiosourse.PlayOneShot(lights);
        lightningTimer = lightningCooldown;  // 重置雷法术的CD
        lightningSkill.ActivateLightningSkill();
    }

    // 火法术：投掷火球，按下2键后点击鼠标左键释放
    void CastFireball()
    {
        fireballaudiosourse.PlayOneShot(fireballs);
        // 重置状态
        fireballTimer = fireballCooldown;  // 重置火法术的CD
        isPreparingFireball = false;  // 取消火法术准备状态

        // 计算鼠标位置
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        fireballSkill = fireball.GetComponent<FireballSkill>();

        // 发射火球
        fireballSkill.CastFireball(mousePosition);
    }

    // 冰法术：冻结周围的敌人
    void CastIce()
    {
        iceaudiosource.PlayOneShot(ices);
        iceTimer = iceCooldown;  // 重置冰法术的CD
        IceSkill.IceSkillCall();
    }

    public void ReduceLightningCooldown()
    {
        lightningCooldown = Mathf.Max(1f, lightningCooldown * 0.9f);  // 限制最小CD
    }

    public void IncreaseLightningDamage()
    {
        lightningSkill.damagePerSecond *= 1.1f ;  // 增加雷电伤害
    }

    public void ExpandLightningRange()
    {
        lightningSkill.effectRadius *= 1.4f;  // 扩大雷电范围
        lightningSkill.damagePerSecond *= 0.95f;
    }

    public void ReduceFireballCooldown()
    {
        fireballCooldown = Mathf.Max(2f, fireballCooldown * 0.9f);
    }

    public void ReduceIceCooldown()
    {
        iceCooldown = Mathf.Max(3f, iceCooldown * 0.9f);
    }

    public void ExtendFreezeDuration()
    {
        IceSkill.freezeDuration *= 1.1f;  // 延长冰冻时间
    }

    public void ExpandFreezeRange()
    {
        IceSkill.freezeRadius *= 1.4f;  // 扩大冰冻范围
        IceSkill.freezeDuration *= 0.9f;
    }

}
