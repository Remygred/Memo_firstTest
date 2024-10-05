using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;  // Ҫ�ػ��Ķ���Ԥ����
    public int initialSize = 20;  // ���г�ʼ��������

    private List<GameObject> pool;  // �洢���ж�����б�

    void Start()
    {
        pool = new List<GameObject>();

        // ��ʼ�������
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewObject();
        }
    }

    // �����¶�����ӵ�����
    private GameObject CreateNewObject()
    {
        GameObject obj = Instantiate(prefab);
        obj.SetActive(false);  // Ԥ����ʱ���������
        pool.Add(obj);
        return obj;
    }

    // ��ȡ���еĿ��ö���
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

        // �������û�п��ö����򴴽�һ���µĶ���
        return CreateNewObject();
    }

    // ������黹������
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);  // ����������ã�����������
    }
}
