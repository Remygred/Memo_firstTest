using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public GameObject upgradePanel;  // 引用升级面板的 UI
    public Button[] optionButtons;   // 三个增幅选项按钮
    public TextMeshProUGUI[] optionTexts;  // 用于显示选项名称的文本
    public Image[] optionIcons;      // 每个按钮对应的图标

    public Sprite[] upgradeIcons;    // 对应升级选项的图标
    public CharacterAtribute character;  // 引用角色属性
    public CharacterControl characterControl;
    public GameObject fireball;  //火球预制体

    public AudioSource audioSource;  // 用于播放按钮点击音效的 AudioSource
    public AudioClip buttonClickSound;  // 按钮点击的音效文件
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

    private System.Action[] upgradeActions;  // 对应升级选项的操作

    void Start()
    {
        upgradePanel.SetActive(false);  // 初始隐藏升级面板

        // 定义对应的升级操作
        upgradeActions = new System.Action[]
        {
            () => { character.IncreaseMaxHp(); },
            () => { character.IncreaseAttack(); },
            () => { character.IncreaseMoveSpeed(); },
            () => { character.IncreaseBulletCapacity(); },
            () => { character.DecreaseReloadTime(); },
            () => { character.ExpandRange(); },
            () => { character.HealAllHP(); },  // 恢复满血
            () => { characterControl.ReduceLightningCooldown(); },  // 减少雷电法术CD
            () => { characterControl.IncreaseLightningDamage(); },  // 提高雷电伤害
            () => { characterControl.ExpandLightningRange(); },  // 扩大雷电范围
            () => { characterControl.ReduceFireballCooldown(); },  // 减少火球法术CD
            () => { fireball.GetComponent<FireballSkill>().IncreaseExplosionDamage(); },  // 提高火球伤害
            () => { fireball.GetComponent<FireballSkill>().IncreaseExplosionRadius(); },  // 扩大火球爆炸范围
            () => { characterControl.ReduceIceCooldown(); },  // 减少冰冻法术CD
            () => { characterControl.ExtendFreezeDuration(); },  // 延长冰冻时间
            () => { characterControl.ExpandFreezeRange(); }  // 扩大冰冻范围
        };

    }

    // 显示升级面板
    public void ShowUpgradePanel()
    {
        Time.timeScale = 0f;  // 暂停游戏
        upgradePanel.SetActive(true);

        // 随机选择三个增幅选项
        List<int> chosenIndexes = new List<int>();
        while (chosenIndexes.Count < 3)
        {
            int index = Random.Range(0, upgrades.Length);
            if (!chosenIndexes.Contains(index))
            {
                chosenIndexes.Add(index);
            }
        }

        // 更新 UI 按钮文本、图标并设置点击监听器
        for (int i = 0; i < 3; i++)
        {
            int upgradeIndex = chosenIndexes[i];
            optionTexts[i].text = upgrades[upgradeIndex];  // 更新文本
            optionIcons[i].sprite = upgradeIcons[upgradeIndex];  // 更新图标

            // 移除旧监听器，确保按钮点击逻辑不会累积
            optionButtons[i].onClick.RemoveAllListeners();
            int capturedIndex = upgradeIndex;  // 捕获当前的索引，避免闭包问题
            optionButtons[i].onClick.AddListener(() => PlayButtonClickSound());  // 播放按钮音效
            optionButtons[i].onClick.AddListener(() => ApplyUpgrade(capturedIndex));  // 执行升级操作
        }
    }

    // 播放按钮点击音效
    void PlayButtonClickSound()
    {
        if (audioSource != null && buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
            audioSource.PlayOneShot(up);    
        }
    }

    // 应用选择的增益
    void ApplyUpgrade(int index)
    {
        upgradeActions[index]();  // 执行相应的增益操作
        HideUpgradePanel();  // 隐藏升级界面并恢复游戏
    }

    // 隐藏升级面板并恢复游戏
    void HideUpgradePanel()
    {
        foreach (Button button in optionButtons)
        {
            button.onClick.RemoveAllListeners();  // 移除所有监听器
        }
        upgradePanel.SetActive(false);  // 隐藏面板
        Time.timeScale = 1f;  // 恢复游戏
    }
}
