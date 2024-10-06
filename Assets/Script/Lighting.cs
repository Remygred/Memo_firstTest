using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;

    void Start()
    {
        offset = player.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position - offset;
    }
}