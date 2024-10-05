using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceOrb : MonoBehaviour
{
    public float moveSpeed = 5f;  // 经验球向玩家移动的速度
    public float absorbDistance ;  // 经验球开始被吸取的距离
    public int experienceValue ;  // 经验球提供的经验值

    private Transform player;  // 玩家对象
    private CharacterAtribute Character;
    private ObjectPool orbPool;  // 对象池引用

    void Start()
    {
        // 查找带有"Player"标签的玩家
        if (player == null || Character == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                Character = playerObj.GetComponent<CharacterAtribute>();
            }
        }

        // 查找对象池
        GameObject poolObject = GameObject.FindGameObjectWithTag("ExpPool");
        if (poolObject != null)
        {
            orbPool = poolObject.GetComponent<ObjectPool>();
        }
    }

    void Update()
    {
        if (player != null)
        {
            // 计算经验球与玩家的距离
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // 如果距离小于吸取范围，经验球向玩家移动
            if (distanceToPlayer <= absorbDistance)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, player.position, step);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 如果经验球碰到玩家，增加玩家的经验值并将经验球返回对象池
        if (other.CompareTag("player"))
        {
            CharacterAtribute playerAtribute = other.GetComponent<CharacterAtribute>();
            if (playerAtribute != null)
            {
                Character.GetExp(experienceValue);
            }

            ReturnToPool();  // 将经验球返回对象池
        }
    }

    void ReturnToPool()
    {
        // 将经验球返回对象池而不是销毁
        if (orbPool != null)
        {
            orbPool.ReturnObject(gameObject);
        }
        else
        {
            Debug.LogError("对象池未找到！");
        }
    }
}
