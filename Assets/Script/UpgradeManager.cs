using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradePanel;  // ������������ UI
    public Button[] optionButtons;   // ��������ѡ�ť
    public TextMeshProUGUI[] optionTexts;  // ������ʾѡ�����Ƶ��ı�
    public Image[] optionIcons;      // ÿ����ť��Ӧ��ͼ��

    public Sprite[] upgradeIcons;    // ��Ӧ����ѡ���ͼ��
    public CharacterAtribute character;  // ���ý�ɫ����
    public CharacterControl characterControl;
    public GameObject fireball;  //����Ԥ����

    public AudioSource audioSource;  // ���ڲ��Ű�ť�����Ч�� AudioSource
    public AudioClip buttonClickSound;  // ��ť�������Ч�ļ�
    public AudioClip up;


    private string[] upgrades = {
        "Increase Max HP<br>(MaxHP + 2)", "Increase Attack<br>(Atk + 2)", "Increase Move Speed<br>(Speed + 20%)",
        "Increase Bullet Capacity<br>(Capacity + 10)","Faster Reload<br>(ReloadTime - 10%)", "Expand the picking range<br>(Range + 10%)", 
        "Heal All HP","Reduce Lightning Cooldown<br>(CD - 10%)", "Increase Lightning Damage<br>(Atk + 10%)",
        "Increase Lightning Range<br>(Range + 10% ,Atk - 5%)","Reduce Fireball Cooldown<br>(CD - 10%)",
        "Increase Fireball Explosion Damage<br>(Atk + 20%)", "Increase Fireball Explosion Radius<br>(Range + 20% ,Atk - 5%)",
        "Reduce Ice Cooldown<br>(CD - 10%)", "Increase Ice Freeze Duration<br>(Duration + 10%)",
        "Increase Ice Freeze Range<br>(Range + 40% ,Durantion - 10%)"
    };

    private System.Action[] upgradeActions;  // ��Ӧ����ѡ��Ĳ���

    void Start()
    {
        upgradePanel.SetActive(false);  // ��ʼ�����������

        // �����Ӧ����������
        upgradeActions = new System.Action[]
        {
            () => { character.IncreaseMaxHp(); },
            () => { character.IncreaseAttack(); },
            () => { character.IncreaseMoveSpeed(); },
            () => { character.IncreaseBulletCapacity(); },
            () => { character.DecreaseReloadTime(); },
            () => { character.ExpandRange(); },
            () => { character.HealAllHP(); },  // �ָ���Ѫ
            () => { characterControl.ReduceLightningCooldown(); },  // �����׵編��CD
            () => { characterControl.IncreaseLightningDamage(); },  // ����׵��˺�
            () => { characterControl.ExpandLightningRange(); },  // �����׵緶Χ
            () => { characterControl.ReduceFireballCooldown(); },  // ���ٻ�����CD
            () => { fireball.GetComponent<FireballSkill>().IncreaseExplosionDamage(); },  // ��߻����˺�
            () => { fireball.GetComponent<FireballSkill>().IncreaseExplosionRadius(); },  // �������ը��Χ
            () => { characterControl.ReduceIceCooldown(); },  // ���ٱ�������CD
            () => { characterControl.ExtendFreezeDuration(); },  // �ӳ�����ʱ��
            () => { characterControl.ExpandFreezeRange(); }  // ���������Χ
        };

    }

    // ��ʾ�������
    public void ShowUpgradePanel()
    {
        Time.timeScale = 0f;  // ��ͣ��Ϸ
        upgradePanel.SetActive(true);

        // ���ѡ����������ѡ��
        List<int> chosenIndexes = new List<int>();
        while (chosenIndexes.Count < 3)
        {
            int index = Random.Range(0, upgrades.Length);
            if (!chosenIndexes.Contains(index))
            {
                chosenIndexes.Add(index);
            }
        }

        // ���� UI ��ť�ı���ͼ�겢���õ��������
        for (int i = 0; i < 3; i++)
        {
            int upgradeIndex = chosenIndexes[i];
            optionTexts[i].text = upgrades[upgradeIndex];  // �����ı�
            optionIcons[i].sprite = upgradeIcons[upgradeIndex];  // ����ͼ��

            // �Ƴ��ɼ�������ȷ����ť����߼������ۻ�
            optionButtons[i].onClick.RemoveAllListeners();
            int capturedIndex = upgradeIndex;  // ����ǰ������������հ�����
            optionButtons[i].onClick.AddListener(() => PlayButtonClickSound());  // ���Ű�ť��Ч
            optionButtons[i].onClick.AddListener(() => ApplyUpgrade(capturedIndex));  // ִ����������
        }
    }

    // ���Ű�ť�����Ч
    void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
            audioSource.PlayOneShot(up);    
        }
    }

    // Ӧ��ѡ�������
    void ApplyUpgrade(int index)
    {
        upgradeActions[index]();  // ִ����Ӧ���������
        HideUpgradePanel();  // �����������沢�ָ���Ϸ
    }

    // ����������岢�ָ���Ϸ
    void HideUpgradePanel()
    {
        foreach (Button button in optionButtons)
        {
            button.onClick.RemoveAllListeners();  // �Ƴ����м�����
        }
        upgradePanel.SetActive(false);  // �������
        Time.timeScale = 1f;  // �ָ���Ϸ
    }
}
