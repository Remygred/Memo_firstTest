using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAtribute : MonoBehaviour
{
    float Hp { get; set; }
    float Atk { get; set; }
    float Exp { get; set; }
    int level { get; set; }
    float MaxExp;
    public float oHp;
    public float oAtk;

    
    public void Start()
    {
        Hp = oHp;
        Atk = oAtk;
        Exp = 0;
        level = 0;
        MaxExp = 10;
    }

    
    public void Update()
    {
        LevelUp();
    }

    void LevelUp()
    {
        if(Exp == MaxExp)
        {
            Exp = 0;
            level++;
            Hp += 5;
            Atk += 1;
        }
        if(level % 5 == 0)
        {
            MaxExp += 10;
        }
    }

    void GetAttack()
    {

    }
}
