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
            // ������ȴ�İٷֱȣ���������ȴ���ֵ������
            cooldownOverlay.fillAmount = CharacterControl.lightningTimer / CharacterControl.lightningCooldown;
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
        }

    }
}
