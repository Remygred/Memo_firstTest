using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    float Move_x;

    public ObjectPool bulletPool;  // ���ö����
    public float bulletSpeed;  // �ӵ������ٶ�
    public float bulletOffset;  // �ӵ�ƫ�ƾ���

    public int magazineSize;  // �ӵ�����
    public int currentAmmo;  // ��ǰ�ӵ���

    public float reloadTime;  // ����CDʱ��
    private bool isReloading = false;  // �Ƿ����ڻ���

    public AudioSource ReloadaudioSource;
    public AudioClip ReloadSound;

    public AudioSource ShotaudioSource;
    public AudioClip ShotSound;

    public CharacterControl CharacterControl;

    void Start()
    {
        offset = player.transform.position - transform.position;
        currentAmmo = magazineSize;
        ShotaudioSource.volume = 0.4f;
    }

    void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        ShotDirect();

        // ������ڻ������������
        if (isReloading) return;

        Move_x = Input.GetAxis("Horizontal");
        if (Move_x > 0)
        {
            transform.position = player.transform.position + offset;
        }
        else
        {
            transform.position = player.transform.position - offset;
        }

        // ���������������һ����ӵ�ʱ�������ӵ�
        if (!CharacterControl.isPreparingFireball && Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Shoot(mousePosition);
            currentAmmo -= 2;  // ÿ���������������ӵ�
        }

        // ����ӵ����꣬��ʼ����
        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
        }
    }

    void ShotDirect()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        Vector3 direction = mousePosition - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 180f;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void Shoot(Vector3 mousePosition)
    {
        if (ShotaudioSource != null && ShotSound != null)
        {
            ShotaudioSource.PlayOneShot(ShotSound);
            ShotaudioSource.PlayOneShot(ShotSound);
        }

        Vector3 direction = (mousePosition - transform.position).normalized;

        Vector3 offset = Vector3.Cross(direction, Vector3.forward) * bulletOffset;

        // ʹ�ö���������ӵ�
        Vector3 leftBulletPosition = transform.position + offset;
        GameObject leftBullet = bulletPool.GetObject();  // �Ӷ���ػ�ȡ�ӵ�
        leftBullet.transform.position = leftBulletPosition;
        leftBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        Rigidbody2D leftRb = leftBullet.GetComponent<Rigidbody2D>();
        leftRb.velocity = direction * bulletSpeed;

        Vector3 rightBulletPosition = transform.position - offset;
        GameObject rightBullet = bulletPool.GetObject();  // �Ӷ���ػ�ȡ�ӵ�
        rightBullet.transform.position = rightBulletPosition;
        rightBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        Rigidbody2D rightRb = rightBullet.GetComponent<Rigidbody2D>();
        rightRb.velocity = direction * bulletSpeed;
    }

    public IEnumerator Reload()
    {
        isReloading = true;  // ���뻻��״̬

        yield return new WaitForSeconds(reloadTime / 2);

        if (ReloadaudioSource != null && ReloadSound != null)
        {
            ReloadaudioSource.PlayOneShot(ReloadSound);
        }

        yield return new WaitForSeconds(reloadTime / 2);  // �ȴ�����CD

        currentAmmo = magazineSize;  // ����װ������
        isReloading = false;  // ��������״̬
    }
}
