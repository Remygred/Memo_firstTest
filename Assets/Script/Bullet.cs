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
        // 如果子弹不再可见，销毁它
        if (!bulletRenderer.isVisible)
        {
            Destroy(gameObject);
        }
    }
}
