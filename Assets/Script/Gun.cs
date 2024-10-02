using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject player;
    Vector3 offset;
    float Move_x;

    public GameObject bulletPrefab;  // �ӵ���Ԥ����
    public float bulletSpeed;  // �ӵ������ٶ�
    public float bulletOffset;  // �ӵ�ƫ�ƾ���

    public int magazineSize;  // �ӵ�����
    public int currentAmmo;  // ��ǰ�ӵ���

    public float reloadTime ;  // ����CDʱ��
    private bool isReloading = false;  // �Ƿ����ڻ���

    void Start()
    {
        offset = player.transform.position - transform.position;
        currentAmmo = magazineSize;
    }

    void Update()
    {
        ShotDirect();

        // ������ڻ������������
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

        // ���������������һ����ӵ�ʱ�������ӵ�
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Shoot(mousePosition);
            currentAmmo--;  // ÿ����������һ���ӵ�
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
        isReloading = true;  // ���뻻��״̬

        yield return new WaitForSeconds(reloadTime);  // �ȴ�����CD

        currentAmmo = magazineSize;  // ����װ������
        isReloading = false;  // ��������״̬
    }
}
