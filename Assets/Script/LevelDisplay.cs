using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    public TextMeshProUGUI levelText;  // 引用文本对象
    private CharacterAtribute player;  // 引用玩家的属性脚本

    void Start()
    {
        // 找到玩家对象
        player = FindObjectOfType<CharacterAtribute>();

        // 初始化等级显示
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        // 动态更新玩家等级
        levelText.text = "Level: " + player.level.ToString();
    }

    void Update()
    {
        // 在游戏中实时更新等级
        UpdateLevelText();
    }
}
