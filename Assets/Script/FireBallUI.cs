using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FireBallUI : MonoBehaviour
{
    public Image skillIcon;
    public Image cooldownOverlay;

    public CharacterControl CharacterControl;

    void Update()
    {

        if (CharacterControl.fireballTimer > 0)
        {
            // 计算冷却的百分比，并更新冷却遮罩的填充量
            cooldownOverlay.fillAmount = CharacterControl.fireballTimer / CharacterControl.fireballCooldown;
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
        }

    }
}
