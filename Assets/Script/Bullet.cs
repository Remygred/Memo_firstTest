using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Renderer bulletRenderer;
    void Start()
    {
        bulletRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // ����ӵ����ٿɼ���������
        if (!bulletRenderer.isVisible)
        {
            Destroy(gameObject);
        }
    }
}
