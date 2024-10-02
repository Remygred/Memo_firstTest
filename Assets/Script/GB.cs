using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D customCursorTexture;  // �Զ�����ͼƬ
    public Vector2 hotspot = Vector2.zero;  // �����ȵ㣨���λ�ã�
    public CursorMode cursorMode = CursorMode.Auto;  // ���ģʽ

    void Start()
    {
        // �����Զ�����
        Cursor.SetCursor(customCursorTexture, hotspot, cursorMode);
    }

    void OnDisable()
    {
        // ���ýű�����ʱ�ָ�Ĭ�Ϲ��
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
