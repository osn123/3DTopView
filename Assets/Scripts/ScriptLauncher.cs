using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLauncher : MonoBehaviour
{
    public GameObject ballPrefab;
    public GameObject player;
    public float spawnDistance = 1.0f; // プレイヤーの前方の生成距離

    private Tutorial3D tutorial3;


    void Start()
    {
        tutorial3 = new Tutorial3D();
        tutorial3.Enable();
    }
    void Update()
    {

        //if (Input.GetMouseButtonDown(1))
        //if (Input.GetKeyDown(KeyCode.PageDown))
        if (tutorial3.Player.Shot.triggered)
        {

            // プレイヤーの位置と前方向を取得
            Vector3 playerPosition = player.transform.position;
            Vector3 playerForward = player.transform.forward;

            // 弾の半径を取得（球体コライダーを想定）
            float bulletRadius = ballPrefab.GetComponent<SphereCollider>().radius;

            // 生成位置を計算（プレイヤーの少し前方）
            Vector3 spawnPosition = playerPosition + (
                playerForward * (spawnDistance + bulletRadius) +
                player.transform.up * (bulletRadius)
                    );

            // ボールを生成
            GameObject spawnedBall = Instantiate(ballPrefab, spawnPosition, player.transform.rotation);

            // ボールの方向を設定
            ScriptBall ballScript = spawnedBall.GetComponent<ScriptBall>();
            if (ballScript != null)
            {
                ballScript.SetDirection(playerForward);
            }

            //Instantiate(ballPrefab, player.transform.position, Quaternion.identity);
            //Instantiate(ballPrefab, player.transform.position, player.transform.rotation);

        }
    }
}
