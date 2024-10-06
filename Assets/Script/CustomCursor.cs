using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;  // 引用自定义光标的纹理
    private Vector2 hotSpot;  // 光标的热点

    void Start()
    {
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);

        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void OnDisable()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
