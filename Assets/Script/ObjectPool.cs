using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;  // 要池化的对象预制体
    public int initialSize = 20;  // 池中初始对象数量

    private List<GameObject> pool;  // 存储池中对象的列表

    void Start()
    {
        pool = new List<GameObject>();

        // 初始化对象池
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    // 创建新对象并添加到池中
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);  // 预生成时将对象禁用
        pool.Add(obj);
        return obj;
    }

    // 获取池中的可用对象
    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        // 如果池中没有可用对象，则创建一个新的对象
        return CreateNewObject();
    }

    // 将对象归还到池中
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);  // 仅将对象禁用，而不是销毁
    }
}
