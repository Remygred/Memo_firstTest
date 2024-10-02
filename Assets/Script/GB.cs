using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D customCursorTexture;  // 自定义光标图片
    public Vector2 hotspot = Vector2.zero;  // 光标的热点（点击位置）
    public CursorMode cursorMode = CursorMode.Auto;  // 光标模式

    void Start()
    {
        // 设置自定义光标
        Cursor.SetCursor(customCursorTexture, hotspot, cursorMode);
    }

    void OnDisable()
    {
        // 当该脚本禁用时恢复默认光标
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
