using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    void Awake()
    {
        // 如果已经有一个 BGMManager 实例存在，销毁这个新的实例
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // 如果没有实例，设定当前实例，并保证它不会被销毁
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
