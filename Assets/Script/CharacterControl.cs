using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public float speed;
    float Move_x;
    float Move_y;
    Animator ani;

    public Gun gunScript;  // ����ǹ�Ľű�
    public ObjectPool bulletPool;  // �����ӵ��Ķ����

    public AudioSource SkillaudioSource;
    public AudioClip SkillSound;

    public AudioSource Walk;
    public AudioClip WalkSound;

    private float curtime = 0f;
    private float settime = 0.3f;

    // ����CDʱ��
    public float skillCooldown = 1.5f;
    public float lightningCooldown = 5f;
    public float fireballCooldown = 7f;
    public float iceCooldown = 10f;

    // ����CD��ʱ��
    public float skillTimer = 0f;
    public float lightningTimer = 0f;
    public float fireballTimer = 0f;
    public float iceTimer = 0f;

    // ������״̬��־
    public bool isPreparingFireball = false;
    public GameObject fireballPrefab;  // ����Ԥ����

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

        // ��ȡ�ӵ������
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

        // ���·���CD��ʱ��
        if(skillTimer > 0)  skillTimer -= Time.deltaTime; 
        if(lightningTimer > 0) lightningTimer -= Time.deltaTime;
        if(fireballTimer > 0) fireballTimer -= Time.deltaTime;
        if(iceTimer > 0) iceTimer -= Time.deltaTime;

        MoveAnimation();
        Move();

        // ��������Ҽ��ļ��ܴ���
        if (Input.GetMouseButtonDown(1) && skillTimer <= 0f)
        {
            UseSkill();
        }
        // ������������
        if (Input.GetKeyDown(KeyCode.Alpha1) && lightningTimer <= 0f)
        {
            CastLightning();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && fireballTimer <= 0f)
        {
            isPreparingFireball = true;  // ׼������
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && iceTimer <= 0f)
        {
            CastIce();
        }

        // �����ڰ����������������ͷ�
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

    // �׷����������е�����ɳ����˺�
    void CastLightning()
    {
        lightaudiosourse.PlayOneShot(lights);
        lightningTimer = lightningCooldown;  // �����׷�����CD
        lightningSkill.ActivateLightningSkill();
    }

    // ������Ͷ�����򣬰���2�������������ͷ�
    void CastFireball()
    {
        fireballaudiosourse.PlayOneShot(fireballs);
        // ����״̬
        fireballTimer = fireballCooldown;  // ���û�����CD
        isPreparingFireball = false;  // ȡ������׼��״̬

        // �������λ��
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        fireballSkill = fireball.GetComponent<FireballSkill>();

        // �������
        fireballSkill.CastFireball(mousePosition);
    }

    // ��������������Χ�ĵ���
    void CastIce()
    {
        iceaudiosource.PlayOneShot(ices);
        iceTimer = iceCooldown;  // ���ñ�������CD
        IceSkill.IceSkillCall();
    }

    public void ReduceLightningCooldown()
    {
        lightningCooldown = Mathf.Max(1f, lightningCooldown * 0.9f);  // ������СCD
    }

    public void IncreaseLightningDamage()
    {
        lightningSkill.damagePerSecond *= 1.1f ;  // �����׵��˺�
    }

    public void ExpandLightningRange()
    {
        lightningSkill.effectRadius *= 1.4f;  // �����׵緶Χ
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
        IceSkill.freezeDuration *= 1.1f;  // �ӳ�����ʱ��
    }

    public void ExpandFreezeRange()
    {
        IceSkill.freezeRadius *= 1.4f;  // ���������Χ
        IceSkill.freezeDuration *= 0.9f;
    }

}
