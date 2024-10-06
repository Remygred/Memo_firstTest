using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public bool isFrozen = false;  // 判断敌人是否被冻结
    private float freezeTimer = 0f;

    public Rigidbody2D rb;  // 用于控制敌人移动的刚体

    private void OnEnable()
    {
        isFrozen = false ;  
        rb.bodyType = RigidbodyType2D.Kinematic;
        freezeTimer = 0;
    }

    void Update()
    {
        // 如果敌人被冻结，则倒计时
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                Unfreeze();  // 冻结时间结束，解除冻结
            }
        }
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        rb.velocity = Vector2.zero;  // 停止敌人的移动
    }

    void Unfreeze()
    {
        isFrozen = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        // 恢复敌人正常行为
    }
}
