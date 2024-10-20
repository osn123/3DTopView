using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScr : MonoBehaviour, ICol
{
    GameObject player;
    public float speed = 1f; // 敵の移動速度

    public void Col(GameObject other)
    {
        Debug.Log("Enemy");
        Destroy(other.gameObject);
        Destroy(gameObject);
        //player = other;
        //Destroy(gameObject);
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void Update()
    {
        if (player != null)
        {
            // プレイヤーの方向へ移動
            Vector3 direction = (player.transform.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // オプション：プレイヤーの方を向く
            transform.LookAt(player.transform);
        }
    }
}