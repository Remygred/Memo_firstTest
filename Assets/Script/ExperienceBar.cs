using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    public Image experienceBarFill;  // ���þ���������䲿��
    private CharacterAtribute player;  // ������ҵ����Խű�

    void Start()
    {
        // ������Ҷ���
        player = FindObjectOfType<CharacterAtribute>();
    }

    void Update()
    {
        // ���ݵ�ǰ�������������辭�������¾�����
        UpdateExperienceBar();
    }

    public void UpdateExperienceBar()
    {
        // ���㾭��������������0��1֮�䣩
        float fillAmount = (float)player.Exp / player.MaxExp;

        // ���¾����������
        experienceBarFill.fillAmount = fillAmount;
    }
}
