using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public Image heartPrefab;  // Ԥ���壬������������ͼ��
    public Transform heartContainer;  // ����ͼ������
    public Sprite fullHeart;  // ����ͼƬ
    public Sprite emptyHeart;  // ����ͼƬ

    public int heartsPerRow;  // ÿ�������ʾ����������
    public float heartSpacingX = 50f;  // ����ͼ��֮���ˮƽ���
    public float heartSpacingY = 50f;  // ����ͼ��֮��Ĵ�ֱ���

    private List<Image> heartImages = new List<Image>();  // �洢�������ɵ����� UI

    // ��ʼ������ͼ��
    public void InitializeHearts(int maxHealth)
    {
        // ����ɵ�����ͼ��
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        // �������Ѫ����������ͼ��
        for (int i = 0; i < maxHealth; i++)
        {
            Image newHeart = Instantiate(heartPrefab, heartContainer);  // �����µ�����ͼ��

            // ���������ͼ���λ��
            int row = i / heartsPerRow;  // ��ǰ����ͼ�����
            int col = i % heartsPerRow;  // ��ǰ����ͼ�����

            RectTransform heartRectTransform = newHeart.GetComponent<RectTransform>();

            // ��������ͼ���λ�ã������ص����������
            heartRectTransform.anchoredPosition = new Vector2(col * heartSpacingX, -row * heartSpacingY);

            heartImages.Add(newHeart);  // �����ɵ�����ͼ����ӵ��б���
        }
    }

    // ��������ͼ��
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeart;  // ���Ѫ�����ڣ���ʾ����
            }
            else
            {
                heartImages[i].sprite = emptyHeart;  // ���Ѫ���۳�����ʾ����
            }
        }
    }
}
