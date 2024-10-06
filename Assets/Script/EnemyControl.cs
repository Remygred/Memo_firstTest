using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public bool isFrozen = false;  // �жϵ����Ƿ񱻶���
    private float freezeTimer = 0f;

    public Rigidbody2D rb;  // ���ڿ��Ƶ����ƶ��ĸ���

    private void OnEnable()
    {
        isFrozen = false ;  
        rb.bodyType = RigidbodyType2D.Kinematic;
        freezeTimer = 0;
    }

    void Update()
    {
        // ������˱����ᣬ�򵹼�ʱ
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                Unfreeze();  // ����ʱ��������������
            }
        }
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        rb.velocity = Vector2.zero;  // ֹͣ���˵��ƶ�
    }

    void Unfreeze()
    {
        isFrozen = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        // �ָ�����������Ϊ
    }
}
