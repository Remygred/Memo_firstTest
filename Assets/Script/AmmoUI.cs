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

    public void UpdateAmmoUI()
    {
        currentAmmoTMP.text = gun.currentAmmo.ToString("D3");

        maxAmmoTMP.text = gun.magazineSize.ToString("D3");
    }
}
