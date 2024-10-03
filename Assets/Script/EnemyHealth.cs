using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int Hp;
    public ExperienceOrbSpawner spawner;  

    //��������
    public void TakeDamage(int damage,bool isBoom)
    {
        Hp -= damage;
        if(Hp <= 0)
        {
            Die(isBoom);
        }
    }

    //��������
    void Die(bool isBoom)
    {
        // ���þ�����������
        spawner =GetComponent<ExperienceOrbSpawner>();
        if (spawner != null && !isBoom)
        {
            // ������˵�����λ�������ɾ�����
            spawner.SpawnExperienceOrbs(transform.position);
        }

        Destroy(gameObject);
    }
}
