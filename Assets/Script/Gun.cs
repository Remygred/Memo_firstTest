using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    float Move_x;

    public ObjectPool bulletPool;  // 引用对象池
    public float bulletSpeed;  // 子弹飞行速度
    public float bulletOffset;  // 子弹偏移距离

    public int magazineSize;  // 子弹容量
    public int currentAmmo;  // 当前子弹数

    public float reloadTime;  // 换弹CD时间
    private bool isReloading = false;  // 是否正在换弹

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

        // 如果正在换弹，则不能射击
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

        // 当按下鼠标左键并且还有子弹时，发射子弹
        if (!CharacterControl.isPreparingFireball && Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Shoot(mousePosition);
            currentAmmo -= 2;  // 每次射击后减少两发子弹
        }

        // 如果子弹用完，开始换弹
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

        // 使用对象池生成子弹
        Vector3 leftBulletPosition = transform.position + offset;
        GameObject leftBullet = bulletPool.GetObject();  // 从对象池获取子弹
        leftBullet.transform.position = leftBulletPosition;
        leftBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        Rigidbody2D leftRb = leftBullet.GetComponent<Rigidbody2D>();
        leftRb.velocity = direction * bulletSpeed;

        Vector3 rightBulletPosition = transform.position - offset;
        GameObject rightBullet = bulletPool.GetObject();  // 从对象池获取子弹
        rightBullet.transform.position = rightBulletPosition;
        rightBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        Rigidbody2D rightRb = rightBullet.GetComponent<Rigidbody2D>();
        rightRb.velocity = direction * bulletSpeed;
    }

    public IEnumerator Reload()
    {
        isReloading = true;  // 进入换弹状态

        yield return new WaitForSeconds(reloadTime / 2);

        if (ReloadaudioSource != null && ReloadSound != null)
        {
            ReloadaudioSource.PlayOneShot(ReloadSound);
        }

        yield return new WaitForSeconds(reloadTime / 2);  // 等待换弹CD

        currentAmmo = magazineSize;  // 重新装满弹夹
        isReloading = false;  // 结束换弹状态
    }
}
