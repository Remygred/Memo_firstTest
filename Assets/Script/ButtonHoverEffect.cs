using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverEffect : MonoBehaviour
{
    public Image buttonImage;  // 引用按钮的Image组件

    public AudioSource SwitchaudioSource;
    public AudioClip SwitchSound;


    // 设置为半透明
    public void SetTransparent()
    {
        if (SwitchaudioSource != null && SwitchSound != null)
        {
            SwitchaudioSource.PlayOneShot(SwitchSound);
        }
        SetButtonAlpha(0.5f);
    }

    // 设置为全透明
    public void SetOpaque()
    {
        SetButtonAlpha(0f);
    }

    // 设置按钮的透明度
    private void SetButtonAlpha(float alpha)
    {
        Color newColor = buttonImage.color;
        newColor.a = alpha;
        buttonImage.color = newColor;
    }
}
