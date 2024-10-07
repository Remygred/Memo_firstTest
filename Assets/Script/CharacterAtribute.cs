using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // 用于场景管理

public class CharacterAtribute : MonoBehaviour
{
    public int Hp;
    public int MaxHp;  // 新增一个用于表示最大血量
    public int Exp;
    public float Atk;
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

    public AudioSource ExpaudioSource;
    public AudioClip ExpSound;

    public AudioSource LevelaudioSource;
    public AudioClip LevelSound;

    public UpgradeManager upgradeManager;  // 引用升级面板管理器
    private bool isDead = false;  // 用于防止重复调用 Die()

    public bool OK = true;

    public CountdownTimer count;

    public GameObject lighting;

    public CharacterControl Control;

    public ExperienceOrb ExpOrb;

    public void Start()
    {
        Hp = oHp;
        MaxHp = oHp;  // 初始化最大血量
        Atk = oAtk;
        Exp = 0;
        level = 0;
        ExpaudioSource.volume = 0.6f;

        // 初始化血量UI
        healthUIManager.InitializeHearts(MaxHp);  // 初始化时生成相应数量的心形图标
        healthUIManager.UpdateHearts(Hp);  // 更新初始的心形图标

        lighting.SetActive(false);
    }

    public void GetExp(int _Exp)
    {
        Exp += _Exp;
        if (ExpaudioSource != null && ExpSound != null)
        {
            ExpaudioSource.PlayOneShot(ExpSound);
        }

        StartCoroutine(LevelUp());  // 使用协程处理升级
        FindObjectOfType<ExperienceBar>().UpdateExperienceBar();
    }

    IEnumerator LevelUp()
    {
        while (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            level++;
            MaxExp *= 2; 

            // 触发闪电动画
            lighting.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            lighting.SetActive(false);

            if (LevelaudioSource != null && LevelSound != null)
            {
                LevelaudioSource.PlayOneShot(LevelSound);
            }

            // 暂停游戏并弹出升级选项面板
            upgradeManager.ShowUpgradePanel();

            // 更新血量上限和当前血量的 UI
            healthUIManager.InitializeHearts(MaxHp);  // 重新生成UI心形图标
            healthUIManager.UpdateHearts(Hp);  // 更新图标显示
        }
    }

    // 增加最大生命值
    public void IncreaseMaxHp()
    {
        MaxHp += 2;
        Hp += 2;
        healthUIManager.InitializeHearts(MaxHp);
        healthUIManager.UpdateHearts(Hp);
    }

    public void HealAllHP()
    {
        Hp = MaxExp;
        healthUIManager.InitializeHearts(MaxHp);
        healthUIManager.UpdateHearts(Hp);
    }

    // 增加攻击力
    public void IncreaseAttack()
    {
        Atk += 2;
    }

    // 增加移动速度
    public void IncreaseMoveSpeed()
    {
        Control.speed *= 1.2f;
    }

    // 增加子弹容量
    public void IncreaseBulletCapacity()
    {
        Gun.magazineSize += 10;
    }

    // 加快换弹速度
    public void DecreaseReloadTime()
    {
        Gun.reloadTime = Mathf.Max(0.5f, Gun.reloadTime * 0.9f);  // 设置最小换弹时间
    }

    public void ExpandRange()
    {
        ExpOrb.absorbDistance *= 1.2f;
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

        if (OK) StartCoroutine(WaitForPause());
    }

    IEnumerator WaitForPause()
    {
        GameObject bgmManager = GameObject.FindWithTag("BGMManager");
        if (bgmManager != null)
        {
            bgmManager.GetComponent<AudioSource>().volume = 0f;
        }
        Animator ani = Death.GetComponent<Animator>();
        ani.SetBool("Die", true);
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
