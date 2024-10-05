using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public TMP_Text currentAmmoTMP;  // 引用显示当前弹药的 TMP Text
    public TMP_Text maxAmmoTMP;      // 引用显示最大弹药的 TMP Text

    public Gun gun;

    // 在游戏开始时初始化弹药 UI
    void Start()
    {
        // 初始化弹药 UI
        UpdateAmmoUI();
    }

    private void Update()
    {
        UpdateAmmoUI();
    }

    // 可以在游戏中当子弹发生变化时调用这个函数
    public void UpdateAmmoUI()
    {
        // 更新当前弹药数的显示，格式化为三位数
        currentAmmoTMP.text = gun.currentAmmo.ToString("D3");

        // 更新最大弹药数的显示，格式化为三位数
        maxAmmoTMP.text = gun.magazineSize.ToString("D3");
    }
}
