using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float lifeTime = 5f;  // �ӵ����������ڣ�����ʱ��
    public int Atk;  // �ӵ���ɵ��˺�
    private CharacterAtribute Character;

    void Start()
    {
        // ��ָ��ʱ��������ӵ�
        Destroy(gameObject, lifeTime);
        if (Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");  // ��ȷ�� Tag Ϊ "player"
            if (playerObj != null)
            {
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }
    }

    // ���ӵ�����������ײ��ʱ
    void OnTriggerEnter2D(Collider2D other)
    {
        // ����������
        if (other.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            // �����ӵ�
            Destroy(gameObject);
            Character.TakeDamage(Atk);
        }else if(other.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            Destroy(gameObject);
        }
    }
}
