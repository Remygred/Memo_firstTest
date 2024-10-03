using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 用于场景管理

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
        if (!IsGetAttack)  // 如果玩家不在无敌状态
        {
            Hp -= damage;  // 扣除生命值
            IsGetAttack = true;  // 设置为无敌状态
            StartCoroutine(GetAttack());  // 启动协程来处理无敌期
        }
        if (Hp <= 0)
        {
            Die();  // 当生命值小于等于 0 时，玩家死亡
        }
    }

    IEnumerator GetAttack()
    {
        yield return new WaitForSeconds(InvincibleTime);  // 等待无敌期结束
        IsGetAttack = false;
    }

    void Die()
    {
        Debug.Log("玩家死亡，游戏结束");

        Time.timeScale = 0f; 
    }
}