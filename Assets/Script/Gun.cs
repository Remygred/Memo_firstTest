using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    float Move_x;

    public GameObject bulletPrefab;  // 子弹的预制体
    public float bulletSpeed;  // 子弹飞行速度
    public float bulletOffset;  // 子弹偏移距离

    public int magazineSize;  // 子弹容量
    public int currentAmmo;  // 当前子弹数

    public float reloadTime ;  // 换弹CD时间
    private bool isReloading = false;  // 是否正在换弹

    void Start()
    {
        offset = player.transform.position - transform.position;
        currentAmmo = magazineSize;
    }

    void Update()
    {
        ShotDirect();

        // 如果正在换弹，则不能射击
        if (isReloading) return;

        Move_x = Input.GetAxis("Horizontal");
        if (Move_x > 0)
        {
            transform.position = player.transform.position + offset;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else
        {
            transform.position = player.transform.position - offset;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        // 当按下鼠标左键并且还有子弹时，发射子弹
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Shoot(mousePosition);
            currentAmmo--;  // 每次射击后减少一发子弹
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
        Vector3 direction = (mousePosition - transform.position).normalized;

        Vector3 offset = Vector3.Cross(direction, Vector3.forward) * bulletOffset;

        Vector3 leftBulletPosition = transform.position + offset;
        GameObject leftBullet = Instantiate(bulletPrefab, leftBulletPosition, Quaternion.identity);
        Rigidbody2D leftRb = leftBullet.GetComponent<Rigidbody2D>();

        leftBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        leftRb.velocity = direction * bulletSpeed;

        Vector3 rightBulletPosition = transform.position - offset;
        GameObject rightBullet = Instantiate(bulletPrefab, rightBulletPosition, Quaternion.identity);
        Rigidbody2D rightRb = rightBullet.GetComponent<Rigidbody2D>();

        rightBullet.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 180f);
        rightRb.velocity = direction * bulletSpeed;
    }

    public IEnumerator Reload()
    {
        isReloading = true;  // 进入换弹状态

        yield return new WaitForSeconds(reloadTime);  // 等待换弹CD

        currentAmmo = magazineSize;  // 重新装满弹夹
        isReloading = false;  // 结束换弹状态
    }
}
