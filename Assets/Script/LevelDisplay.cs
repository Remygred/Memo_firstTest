using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelDisplay : MonoBehaviour
{
    public TextMeshProUGUI levelText;  // �����ı�����
    private CharacterAtribute player;  // ������ҵ����Խű�

    void Start()
    {
        // �ҵ���Ҷ���
        player = FindObjectOfType<CharacterAtribute>();

        // ��ʼ���ȼ���ʾ
        UpdateLevelText();
    }

    void UpdateLevelText()
    {
        // ��̬������ҵȼ�
        levelText.text = "Level: " + player.level.ToString();
    }

    void Update()
    {
        // ����Ϸ��ʵʱ���µȼ�
        UpdateLevelText();
    }
}
