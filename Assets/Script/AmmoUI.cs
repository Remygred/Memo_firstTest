using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
    public TMP_Text currentAmmoTMP;  // ������ʾ��ǰ��ҩ�� TMP Text
    public TMP_Text maxAmmoTMP;      // ������ʾ���ҩ�� TMP Text

    public Gun gun;

    // ����Ϸ��ʼʱ��ʼ����ҩ UI
    void Start()
    {
        // ��ʼ����ҩ UI
        UpdateAmmoUI();
    }

    private void Update()
    {
        UpdateAmmoUI();
    }

    // ��������Ϸ�е��ӵ������仯ʱ�����������
    public void UpdateAmmoUI()
    {
        // ���µ�ǰ��ҩ������ʾ����ʽ��Ϊ��λ��
        currentAmmoTMP.text = gun.currentAmmo.ToString("D3");

        // �������ҩ������ʾ����ʽ��Ϊ��λ��
        maxAmmoTMP.text = gun.magazineSize.ToString("D3");
    }
}
