using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptPlayer : MonoBehaviour
{
    Rigidbody rigid;
    float jump_speed = 8.0f; // ジャンプの度合い

    private Tutorial3D tutorial3;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        rigid = GetComponent<Rigidbody>();

        tutorial3 = new Tutorial3D();
        tutorial3.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // マウス左ボタンが押されたら
        //if (Input.GetMouseButtonDown(0))
        //if (Input.GetKeyDown(KeyCode.PageUp))
        if (tutorial3.Player.Generate.triggered)
        {
            // ジャンプをする
            rigid.velocity = Vector3.up * jump_speed;
        }
    }
}
