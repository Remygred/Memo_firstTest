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
            // ������ȴ�İٷֱȣ���������ȴ���ֵ������
            cooldownOverlay.fillAmount = CharacterControl.fireballTimer / CharacterControl.fireballCooldown;
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
        }

    }
}
