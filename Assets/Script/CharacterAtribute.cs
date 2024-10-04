using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 用于场景管理

public class CharacterAtribute : MonoBehaviour
{
    public int Hp;
    public int MaxHp;  // 新增一个用于表示最大血量
    public int Exp;
    public int Atk;
    public int level;
    public int MaxExp;
    public int oHp;
    public int oAtk;

    public float InvincibleTime;
    public bool IsGetAttack = false;

    public Gun Gun;

    public HealthUIManager healthUIManager;  // 引用 HealthUIManager 脚本
    public GameObject Death;

    public AudioSource audioSource;  // 用于播放音效的 AudioSource
    public AudioClip deathSound;     // 死亡音效

    public AudioSource HealaudioSource;  
    public AudioClip HealSound;

    public AudioSource ExpaudioSource;
    public AudioClip ExpSound;

    public AudioSource LevelaudioSource;
    public AudioClip LevelSound;

    private bool isDead = false;  // 用于防止重复调用 Die()

    public bool OK = true;

    public CountdownTimer count;

    public void Start()
    {
        Hp = oHp;
        MaxHp = oHp;  // 初始化最大血量
        Atk = oAtk;
        Exp = 0;
        level = 0;
        MaxExp = 10;
        ExpaudioSource.volume = 0.6f;

        // 初始化血量UI
        healthUIManager.InitializeHearts(MaxHp);  // 初始化时生成相应数量的心形图标
        healthUIManager.UpdateHearts(Hp);  // 更新初始的心形图标
    }

    public void GetExp(int _Exp)
    {
        Exp += _Exp;
        if (ExpaudioSource != null && ExpSound != null)
        {
            ExpaudioSource.PlayOneShot(ExpSound);
        }
        LevelUp();
        // 更新经验条
        FindObjectOfType<ExperienceBar>().UpdateExperienceBar();
    }

    void LevelUp()
    {
        while (Exp >= MaxExp)
        {
            if (LevelaudioSource != null && LevelSound != null)
            {
                LevelaudioSource.PlayOneShot(LevelSound);
            }
            Exp -= MaxExp;
            level++;
            oHp += 5;
            Gun.magazineSize++;
            if (Hp == MaxHp)
            {
                Hp += 1;
                MaxHp += 1;  // 增加最大血量
            } 
            else 
            {
                Hp++;
                if (HealaudioSource != null && HealSound != null)
                {
                    HealaudioSource.PlayOneShot(HealSound);
                }
            }
            Atk += 1;

            // 更新血量上限和当前血量的 UI
            healthUIManager.InitializeHearts(MaxHp);  // 重新生成UI心形图标
            healthUIManager.UpdateHearts(Hp);  // 更新图标显示
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsGetAttack)  // 如果玩家不在无敌状态
        {
            Hp -= damage;  // 扣除生命值
            IsGetAttack = true;  // 设置为无敌状态
            StartCoroutine(GetAttack());  // 启动协程来处理无敌期

            healthUIManager.UpdateHearts(Hp);  // 每次受伤时更新心形图标
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
        if (isDead) return;  // 防止多次调用 Die()

        isDead = true;  // 标记角色为已死亡

        count.OK = false;

        if(OK) StartCoroutine(WaitForPause());  
    }

    IEnumerator WaitForPause()
    {
        GameObject bgmManager = GameObject.FindWithTag("BGMManager");
        if (bgmManager != null)
        {
            bgmManager.GetComponent<AudioSource>().volume = 0f;
        }
        Animator ani = Death.GetComponent<Animator>();
        ani.SetBool("Die",true);
        // 播放死亡音效
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        yield return new WaitForSecondsRealtime(1f);
        bgmManager.GetComponent<AudioSource>().volume = 1f;
        Time.timeScale = 0f;  // 暂停游戏
    }

    public void Heal(int amount)
    {
        Hp += amount;
        if (Hp > MaxHp)
            Hp = MaxHp;

        healthUIManager.UpdateHearts(Hp);  // 治疗后更新心形图标
    }
}
