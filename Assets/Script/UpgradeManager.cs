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

    public AudioSource audioSource;  // ���ڲ��Ű�ť�����Ч�� AudioSource
    public AudioClip buttonClickSound;  // ��ť�������Ч�ļ�
    public AudioClip up;


    private string[] upgrades = { "Increase Max HP", "Increase Attack", "Increase Move Speed", "Increase Bullet Capacity", "Faster Reload", "Expand the picking range" };
    private System.Action[] upgradeActions;  // ��Ӧ����ѡ��Ĳ���

    void Start()
    {
        upgradePanel.SetActive(false);  // ��ʼ�����������

        // �����Ӧ����������
        upgradeActions = new System.Action[]
        {
            () => { character.IncreaseMaxHp(); },  // �����������ֵ
            () => { character.IncreaseAttack(); },  // ���ӹ�����
            () => { character.IncreaseMoveSpeed(); },  // �����ƶ��ٶ�
            () => { character.IncreaseBulletCapacity(); },  // �����ӵ�����
            () => { character.DecreaseReloadTime(); },  // �ӿ컻���ٶ�
            () => { character.ExpandRange(); }  // ������ȡ��Χ
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
