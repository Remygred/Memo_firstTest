using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIManager : MonoBehaviour
{
    public Image heartPrefab;  // 预制体，用于生成心形图标
    public Transform heartContainer;  // 心形图标容器
    public Sprite fullHeart;  // 红心图片
    public Sprite emptyHeart;  // 黑心图片

    public int heartsPerRow;  // 每行最多显示的心形数量
    public float heartSpacingX = 50f;  // 心形图标之间的水平间距
    public float heartSpacingY = 50f;  // 心形图标之间的垂直间距

    private List<Image> heartImages = new List<Image>();  // 存储所有生成的心形 UI

    // 初始化心形图标
    public void InitializeHearts(int maxHealth)
    {
        // 清除旧的心形图标
        foreach (Transform child in heartContainer)
        {
            Destroy(child.gameObject);
        }
        heartImages.Clear();

        // 根据最大血量生成心形图标
        for (int i = 0; i < maxHealth; i++)
        {
            Image newHeart = Instantiate(heartPrefab, heartContainer);  // 生成新的心形图标

            // 计算该心形图标的位置
            int row = i / heartsPerRow;  // 当前心形图标的行
            int col = i % heartsPerRow;  // 当前心形图标的列

            RectTransform heartRectTransform = newHeart.GetComponent<RectTransform>();

            // 设置心形图标的位置，避免重叠，溢出换行
            heartRectTransform.anchoredPosition = new Vector2(col * heartSpacingX, -row * heartSpacingY);

            heartImages.Add(newHeart);  // 将生成的心形图标添加到列表中
        }
    }

    // 更新心形图标
    public void UpdateHearts(int currentHealth)
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            if (i < currentHealth)
            {
                heartImages[i].sprite = fullHeart;  // 如果血量还在，显示红心
            }
            else
            {
                heartImages[i].sprite = emptyHeart;  // 如果血量扣除，显示黑心
            }
        }
    }
}
