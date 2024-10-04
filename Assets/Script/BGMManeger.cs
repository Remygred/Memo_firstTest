using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    void Awake()
    {
        // ����Ѿ���һ�� BGMManager ʵ�����ڣ���������µ�ʵ��
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // ���û��ʵ�����趨��ǰʵ��������֤�����ᱻ����
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
