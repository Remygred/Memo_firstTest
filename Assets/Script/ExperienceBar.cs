using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Image experienceBarFill;  // 引用经验条的填充部分
    private CharacterAtribute player;  // 引用玩家的属性脚本

    void Start()
    {
        // 查找玩家对象
        player = FindObjectOfType<CharacterAtribute>();
    }

    void Update()
    {
        // 根据当前经验与升级所需经验来更新经验条
        UpdateExperienceBar();
    }

    public void UpdateExperienceBar()
    {
        // 计算经验条的填充比例（0到1之间）
        float fillAmount = (float)player.Exp / player.MaxExp;

        // 更新经验条填充量
        experienceBarFill.fillAmount = fillAmount;
    }
}
