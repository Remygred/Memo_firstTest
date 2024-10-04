using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour
{
    public Image buttonImage;  // ���ð�ť��Image���

    public AudioSource SwitchaudioSource;
    public AudioClip SwitchSound;


    // ����Ϊ��͸��
    public void SetTransparent()
    {
        if (SwitchaudioSource != null && SwitchSound != null)
        {
            SwitchaudioSource.PlayOneShot(SwitchSound);
        }
        SetButtonAlpha(0.5f);
    }

    // ����Ϊȫ͸��
    public void SetOpaque()
    {
        SetButtonAlpha(0f);
    }

    // ���ð�ť��͸����
    private void SetButtonAlpha(float alpha)
    {
        Color newColor = buttonImage.color;
        newColor.a = alpha;
        buttonImage.color = newColor;
    }
}
