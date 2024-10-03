using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // ���ڳ�������

public class CharacterAtribute : MonoBehaviour
{
    public int Hp;
    public int Exp;
    public int Atk;
    public int level;
    public int MaxExp;
    public int oHp;
    public int oAtk;

    public float InvincibleTime;
    public bool IsGetAttack = false;


    public void Start()
    {
        Hp = oHp;
        Atk = oAtk;
        Exp = 0;
        level = 0;
        MaxExp = 10;
    }

    public void GetExp(int _Exp)
    {
        Exp += _Exp;
        LevelUp();
    }

    void LevelUp()
    {
        while (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            level++;
            oHp += 5;
            Hp += 5;
            Atk += 1;
            if (level != 0 && level % 5 == 0)
            {
                MaxExp += 10;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsGetAttack)  // �����Ҳ����޵�״̬
        {
            Hp -= damage;  // �۳�����ֵ
            IsGetAttack = true;  // ����Ϊ�޵�״̬
            StartCoroutine(GetAttack());  // ����Э���������޵���
        }
        if (Hp <= 0)
        {
            Die();  // ������ֵС�ڵ��� 0 ʱ���������
        }
    }

    IEnumerator GetAttack()
    {
        yield return new WaitForSeconds(InvincibleTime);  // �ȴ��޵��ڽ���
        IsGetAttack = false;
    }

    void Die()
    {
        Debug.Log("�����������Ϸ����");

        Time.timeScale = 0f; 
    }
}