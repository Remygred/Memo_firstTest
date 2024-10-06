using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightingSkillUI : MonoBehaviour
{
    public Image skillIcon;
    public Image cooldownOverlay;

    public CharacterControl CharacterControl;

    void Update()
    {

        if (CharacterControl.lightningTimer > 0)
        {
            // 计算冷却的百分比，并更新冷却遮罩的填充量
            cooldownOverlay.fillAmount = CharacterControl.lightningTimer / CharacterControl.lightningCooldown;
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
        }

    }
}
