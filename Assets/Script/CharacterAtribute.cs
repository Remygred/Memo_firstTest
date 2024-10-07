using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // ���ڳ�������

public class CharacterAtribute : MonoBehaviour
{
    public int Hp;
    public int MaxHp;  // ����һ�����ڱ�ʾ���Ѫ��
    public int Exp;
    public float Atk;
    public int level;
    public int MaxExp;
    public int oHp;
    public int oAtk;

    public float InvincibleTime;
    public bool IsGetAttack = false;

    public Gun Gun;

    public HealthUIManager healthUIManager;  // ���� HealthUIManager �ű�
    public GameObject Death;

    public AudioSource audioSource;  // ���ڲ�����Ч�� AudioSource
    public AudioClip deathSound;     // ������Ч

    public AudioSource ExpaudioSource;
    public AudioClip ExpSound;

    public AudioSource LevelaudioSource;
    public AudioClip LevelSound;

    public UpgradeManager upgradeManager;  // ����������������
    private bool isDead = false;  // ���ڷ�ֹ�ظ����� Die()

    public bool OK = true;

    public CountdownTimer count;

    public GameObject lighting;

    public CharacterControl Control;

    public ExperienceOrb ExpOrb;

    public void Start()
    {
        Hp = oHp;
        MaxHp = oHp;  // ��ʼ�����Ѫ��
        Atk = oAtk;
        Exp = 0;
        level = 0;
        ExpaudioSource.volume = 0.6f;

        // ��ʼ��Ѫ��UI
        healthUIManager.InitializeHearts(MaxHp);  // ��ʼ��ʱ������Ӧ����������ͼ��
        healthUIManager.UpdateHearts(Hp);  // ���³�ʼ������ͼ��

        lighting.SetActive(false);
    }

    public void GetExp(int _Exp)
    {
        Exp += _Exp;
        if (ExpaudioSource != null && ExpSound != null)
        {
            ExpaudioSource.PlayOneShot(ExpSound);
        }

        StartCoroutine(LevelUp());  // ʹ��Э�̴�������
        FindObjectOfType<ExperienceBar>().UpdateExperienceBar();
    }

    IEnumerator LevelUp()
    {
        while (Exp >= MaxExp)
        {
            Exp -= MaxExp;
            level++;
            MaxExp *= 2; 

            // �������綯��
            lighting.SetActive(true);
            yield return new WaitForSeconds(0.25f);
            lighting.SetActive(false);

            if (LevelaudioSource != null && LevelSound != null)
            {
                LevelaudioSource.PlayOneShot(LevelSound);
            }

            // ��ͣ��Ϸ����������ѡ�����
            upgradeManager.ShowUpgradePanel();

            // ����Ѫ�����޺͵�ǰѪ���� UI
            healthUIManager.InitializeHearts(MaxHp);  // ��������UI����ͼ��
            healthUIManager.UpdateHearts(Hp);  // ����ͼ����ʾ
        }
    }

    // �����������ֵ
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

    // ���ӹ�����
    public void IncreaseAttack()
    {
        Atk += 2;
    }

    // �����ƶ��ٶ�
    public void IncreaseMoveSpeed()
    {
        Control.speed *= 1.2f;
    }

    // �����ӵ�����
    public void IncreaseBulletCapacity()
    {
        Gun.magazineSize += 10;
    }

    // �ӿ컻���ٶ�
    public void DecreaseReloadTime()
    {
        Gun.reloadTime = Mathf.Max(0.5f, Gun.reloadTime * 0.9f);  // ������С����ʱ��
    }

    public void ExpandRange()
    {
        ExpOrb.absorbDistance *= 1.2f;
    }

    public void TakeDamage(int damage)
    {
        if (!IsGetAttack)  // �����Ҳ����޵�״̬
        {
            Hp -= damage;  // �۳�����ֵ
            IsGetAttack = true;  // ����Ϊ�޵�״̬
            StartCoroutine(GetAttack());  // ����Э���������޵���

            healthUIManager.UpdateHearts(Hp);  // ÿ������ʱ��������ͼ��
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
        if (isDead) return;  // ��ֹ��ε��� Die()

        isDead = true;  // ��ǽ�ɫΪ������

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
        // ����������Ч
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        yield return new WaitForSecondsRealtime(1f);
        bgmManager.GetComponent<AudioSource>().volume = 1f;
        Time.timeScale = 0f;  // ��ͣ��Ϸ
    }

    public void Heal(int amount)
    {
        Hp += amount;
        if (Hp > MaxHp)
            Hp = MaxHp;

        healthUIManager.UpdateHearts(Hp);  // ���ƺ��������ͼ��
    }
}
