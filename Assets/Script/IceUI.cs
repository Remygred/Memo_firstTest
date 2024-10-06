using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IceUI : MonoBehaviour
{
    public Image skillIcon;
    public Image cooldownOverlay;

    public CharacterControl CharacterControl;

    void Update()
    {

        if (CharacterControl.iceTimer > 0)
        {
            // ������ȴ�İٷֱȣ���������ȴ���ֵ������
            cooldownOverlay.fillAmount = CharacterControl.iceTimer / CharacterControl.iceCooldown;
        }
        else
        {
            cooldownOverlay.fillAmount = 0f;
        }

    }
}
