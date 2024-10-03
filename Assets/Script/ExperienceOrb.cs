using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public float moveSpeed = 5f;  // ������������ƶ����ٶ�
    public float absorbDistance = 3f;  // ������ʼ����ȡ�ľ���
    public int experienceValue = 10;  // �������ṩ�ľ���ֵ

    private Transform player;  // ��Ҷ���
    private CharacterAtribute Character;

    void Start()
    {
        // ���Ҵ���"Player"��ǩ�����
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player"); 
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }
    }

    void Update()
    {
        if (player != null)
        {
            // ���㾭��������ҵľ���
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // �������С����ȡ��Χ��������������ƶ�
            if (distanceToPlayer <= absorbDistance)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // ���������������ң�������ҵľ���ֵ�����پ�����
        if (other.CompareTag("player"))
        {
            CharacterAtribute playerAtribute = other.GetComponent<CharacterAtribute>();
            if (playerAtribute != null)
            {
                Character.GetExp(experienceValue);
            }

            Destroy(gameObject);  // ���پ�����
        }
    }
}
