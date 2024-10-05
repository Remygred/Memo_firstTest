using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeEnemy : MonoBehaviour
{
    public Transform player;  // 玩家对象的Transform
    public float eyeOpenDistance;  // 玩家靠近到此距离时树睁眼
    public int contactDamage;  // 玩家碰到树时受到的伤害
    private bool isEyeOpen = false;  // 树是否睁眼
    private Animator animator;  // 动画控制器

    public int Atk;
    private CharacterAtribute Character;

    public AudioSource AtkaudioSource;
    public AudioClip AtkSound;
    void Start()
    {
        // 如果没有手动设置玩家对象，自动查找
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }

        // 获取树的动画组件
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // 计算玩家与树的距离
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // 如果玩家靠近到一定距离，树睁眼
        if (distanceToPlayer <= eyeOpenDistance && !isEyeOpen)
        {
            OpenEyes();
        }
        // 玩家远离树，关闭眼睛
        else if (distanceToPlayer > eyeOpenDistance && isEyeOpen)
        {
            CloseEyes();
        }
    }

    // 当玩家碰到树时触发
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            if (AtkaudioSource != null && AtkSound != null)
            {
                AtkaudioSource.PlayOneShot(AtkSound);
            }
            animator.SetBool("Attack",true);
            Character.TakeDamage(Atk);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("player") && !Character.IsGetAttack)
        {
            if (AtkaudioSource != null && AtkSound != null)
            {
                AtkaudioSource.PlayOneShot(AtkSound);
            }
            animator.SetBool("Attack", true);
            Character.TakeDamage(Atk);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("player"))
        {
            animator.SetBool("Attack", false);
        }
    }

    // 树睁眼
    void OpenEyes()
    {
        isEyeOpen = true;
        animator.SetBool("Player_Close", true);  // 播放睁眼动画
    }

    // 树关闭眼睛
    void CloseEyes()
    {
        isEyeOpen = false;
        animator.SetBool("Player_Close", false);  // 播放闭眼动画
    }
}
