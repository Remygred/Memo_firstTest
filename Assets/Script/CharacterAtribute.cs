using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // ���ڳ�������

public class CharacterAtribute : MonoBehaviour
{
    public int Hp;
    public int MaxHp;  // ����һ�����ڱ�ʾ���Ѫ��
    public int Exp;
    public int Atk;
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

    public AudioSource HealaudioSource;  
    public AudioClip HealSound;

    public AudioSource ExpaudioSource;
    public AudioClip ExpSound;

    public AudioSource LevelaudioSource;
    public AudioClip LevelSound;

    private bool isDead = false;  // ���ڷ�ֹ�ظ����� Die()

    public bool OK = true;

    public CountdownTimer count;

    public void Start()
    {
        Hp = oHp;
        MaxHp = oHp;  // ��ʼ�����Ѫ��
        Atk = oAtk;
        Exp = 0;
        level = 0;
        MaxExp = 10;
        ExpaudioSource.volume = 0.6f;

        // ��ʼ��Ѫ��UI
        healthUIManager.InitializeHearts(MaxHp);  // ��ʼ��ʱ������Ӧ����������ͼ��
        healthUIManager.UpdateHearts(Hp);  // ���³�ʼ������ͼ��
    }

    public void GetExp(int _Exp)
    {
        Exp += _Exp;
        if (ExpaudioSource != null && ExpSound != null)
        {
            ExpaudioSource.PlayOneShot(ExpSound);
        }
        LevelUp();
        // ���¾�����
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
                MaxHp += 1;  // �������Ѫ��
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

            // ����Ѫ�����޺͵�ǰѪ���� UI
            healthUIManager.InitializeHearts(MaxHp);  // ��������UI����ͼ��
            healthUIManager.UpdateHearts(Hp);  // ����ͼ����ʾ
        }
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
