using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.InputSystem.XR;
using TMPro;


public class PlayerController : Character
{
    private Rigidbody2D player;
    public float player_speed;
    public float player_jump;
    private float xInput;
    private Animator anim;
    private int facingDir = 1;
    public bool facingRight = true;
    GameObject ChangeC;
    GameObject ChangeB;
    public float damage;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    public float UnderGroundRayLength = 0.5f;
    private bool isUnderGround = false;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;

    private bool isDefending = false;
    public float defenseDuration = 2;
    public float defenseCooldown = 1;
    private bool canDefend = true;
    private bool isRespawning;
    private float respawnTime = 0.0f;
    private float respawnDuration;

    public GameObject weapon; // 添加武器对象

    public Transform skillTf;
    public Transform RayTf;

    public bool isDead = false;

    Vector2 checkpointPos;

    AudioSource audioS;
    AudioSource audioS2;
    AudioSource audioS3;
    public AudioClip jumpclip;
    public AudioClip skillclip;
    public AudioClip deadclip;
    public AudioClip hurtclip;
    public AudioClip itemclip;
    public AudioClip fireclip;

    public TextMeshProUGUI tmp;

    private struct PlayerState
    {
        public Vector2 position;
        public float Health;
    }

    private PlayerState savedState;

    private PLayerInputAction controls;
    private Vector2 move;


    private void Awake()
    {
        controls = new PLayerInputAction();

        //controls.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        //controls.Player.Move.canceled += ctx => move = Vector2.zero;

        controls.Player.Jump.started += ctx => jump();
        if (canDefend) { controls.Player.Skill.started += ctx => StartCoroutine(Defend()); }
    }



    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        skillTf = transform.Find("Skill");
        ChangeB = GameObject.Find("Main Camera");
        ChangeC = GameObject.Find("Change");

        savedState = new PlayerState
        {
            position = checkpointPos,
            Health = currentHealth
        };
        checkpointPos = transform.position;

        if (audioS == null)
        {
            audioS = GetComponent<AudioSource>();
        }
        if (audioS2 == null)
        {
            AudioSource[] audioSources = GetComponents<AudioSource>();
            if (audioSources.Length > 1) audioS2 = audioSources[1];
            if (audioSources.Length > 2) audioS3 = audioSources[2];
        }
    }

    void Update()
    {
        movement();
        HandleInput();
        GroundCollisionCheck();
        animatorcontrollers();
        FlipController();
        //Mutekimode();

        isRespawningOR();
        if (currentHealth == 0)
        {
            Respawn(2.5f);
        }
    }

    public void CheckSkillBox()
    {
        float width = 10;
        float height = 10;
        Vector2 pos1 = skillTf.position + skillTf.right * width * 0.5f + skillTf.up * height * 0.5f;
        Vector2 pos2 = skillTf.position - skillTf.right * width * 0.5f + skillTf.up * height * 0.5f;
        Vector2 pos3 = skillTf.position + skillTf.right * width * 0.5f - skillTf.up * height * 0.5f;
        Vector2 pos4 = skillTf.position - skillTf.right * width * 0.5f - skillTf.up * height * 0.5f;
        Debug.DrawLine(pos1, pos2, Color.red, 0.25f);
        Debug.DrawLine(pos2, pos4, Color.red, 0.25f);
        Debug.DrawLine(pos3, pos4, Color.red, 0.25f);
        Debug.DrawLine(pos3, pos1, Color.red, 0.25f);

        //box collision
        /*
                Collider2D col = Physics2D.OverlapBox(skillTf.position, new Vector2(width, height), 0, LayerMask.GetMask("Bullet"));
                if (col != null)
                {
                    Bullet b1 = col.GetComponent<Bullet>();
                    if(b1 != null)
                    {
                        b1.Flip();
                    }

                    Bullet2 b2 = col.GetComponent<Bullet2>();
                    if(b2!=null)
                    {
                        b2.Flip();
                    }
                }
        */
        Collider2D[] cols = Physics2D.OverlapBoxAll(skillTf.position, new Vector2(width, height), 0, LayerMask.GetMask("Bullet"));
        for (int i = 0; i < cols.Length; ++i)
        {
            Collider2D col = cols[i];
            if (col != null)
            {
                Bullet b1 = col.GetComponent<Bullet>();
                if (b1 != null)
                {
                    b1.Flip();
                }

                Bullet2 b2 = col.GetComponent<Bullet2>();
                if (b2 != null)
                {
                    b2.Flip();
                }
            }
        }

        Collider2D col2 = Physics2D.OverlapBox(skillTf.position, new Vector2(width, height), 0, LayerMask.GetMask("Enemy"));
        if (col2 != null)
        {
            col2.GetComponent<Character>().TakeDamage(damage);
        }
    }

    private void GroundCollisionCheck()
    {
        isGrounded = Physics2D.Raycast(RayTf.transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isUnderGround = Physics2D.Raycast(RayTf.transform.position, Vector2.down, UnderGroundRayLength, whatIsGround);
        if (isUnderGround == true)
        {
            Vector2 currentPosition = player.position;// 更新 y 坐标
            currentPosition.y += 0.8f; // 将 y 坐标加 10
            player.position = currentPosition;// 重新设置玩家的位置
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(RayTf.transform.position, new Vector3(RayTf.transform.position.x, RayTf.transform.position.y - groundCheckDistance));
    }
    private void HandleInput()
    {
        if (isDead == false)
        {
            if (Input.GetButtonDown("Jump"))
            {
                jump();

            }

            xInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Skill") && canDefend)
            {
                StartCoroutine(Defend());
                audioS2.clip = skillclip;
                audioS2.Play();
            }
        }
        else
        {
            player.velocity = Vector3.zero;
        }
    }

    private void movement()
    {
        player.velocity = new Vector2(xInput * player_speed, player.velocity.y);
    }

    public void jump()
    {
        Debug.Log("jump");
        if (isGrounded)
        {
            player.velocity = new Vector2(player.velocity.x, player_jump);
            audioS.clip = jumpclip;
            audioS.Play();
        }

    }

    //private void Mutekimode()
    //{
    //    if (Input.GetButtonDown("Muteki"))
    //    {
    //        currentHealth = 10000;
    //    }
    //}

    private void animatorcontrollers()
    {
        bool isMoving = player.velocity.x != 0;

        anim.SetFloat("yVelocity", player.velocity.y);
        anim.SetBool("ismoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isDefending", isDefending);
        anim.SetBool("isDead", isDead);
    }

    private void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void FlipController()
    {
        if (player.velocity.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (player.velocity.x < 0 && facingRight)
        {
            Flip();
        }
    }



    private IEnumerator Defend()
    {
        isDefending = true;
        canDefend = false;
        yield return new WaitForSeconds(defenseDuration);
        isDefending = false;
        yield return new WaitForSeconds(defenseCooldown);
        canDefend = true;

    }

    public bool IsDefending()
    {
        return isDefending;
    }


    public void PlayerHurt()
    {
        anim.SetTrigger("hurt");
        audioS.clip = hurtclip;
        audioS.Play();
    }

    public void PlayerDie()
    {
        isDead = true;
        if (!audioS.isPlaying)
        {
            audioS.clip = deadclip;
            audioS.PlayOneShot(deadclip);
        }
    }

    public void changeB()
    {
        if (ChangeB != null)
        {
            InvertCircleController C = ChangeB.GetComponent<InvertCircleController>();
            C.Change();
        }
    }


    private void Respawn(float duration)
    {
        isRespawning = true;
        respawnDuration = duration;
    }

    private void isRespawningOR()
    {
        if (isRespawning)
        {
            Debug.Log(respawnTime);
            respawnTime += Time.deltaTime;
            if (respawnTime > respawnDuration)
            {
                Debug.Log("res222");
                RestoreState();
                isDead = false; // 重置死亡状态
                anim.SetBool("isDead", false); // 重置动画参数
                anim.Play("Idle"); // 强制播放Idle动画，避免停留在死亡动画上
                respawnTime = 0.0f;
                isRespawning = false;
            }
        }
    }

    public void UpDateCheckPoint(Vector2 pos)
    {
        checkpointPos = pos;
        currentHealth = maxHealth;
        SaveState();
    }

    public void SaveState()
    {
        savedState.position = transform.position;
        savedState.Health = currentHealth;
    }

    public void RestoreState()
    {
        transform.position = savedState.position;
        currentHealth = savedState.Health;
    }
    public float GetHP()
    {
        return currentHealth;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "GoalItem")
        {
            audioS3.clip = itemclip;
            audioS3.Play();
        }
        if (collision.gameObject.tag == "CheckPoint")
        {
            audioS.clip = fireclip;
            audioS.Play();
        }

        if (collision.gameObject.name == "SkillTeaching")
        {
            tmp.text = "RBを押して能力を使ってみよう";
        }
        if (collision.gameObject.name == "CheckPoint")
        {
            tmp.text = "今までの旅をここで記録しよう";
        }
        if (collision.gameObject.name == "ShardTeaching")
        {
            tmp.text = "目の前の宝石を取って、かがり火の色を元に戻そう！";
        }
        if (collision.gameObject.name == "JumpTeaching")
        {
            tmp.text = "Aを押してジャンプ！";
        }
        if (collision.gameObject.name == "MoveTeaching")
        {
            tmp.text = "LStickを使って移動してみよう！";
        }
        if (collision.gameObject.name == "SkillTeaching2")
        {
            tmp.text = "困ったら「能力」を使ってみよう！";
        }
        if (collision.gameObject.name == "GuardTeaching")
        {
            tmp.text = "能力を使って敵の弾の弾き返そう！";
        }
        if (collision.gameObject.name == "FightTeaching")
        {
            tmp.text = "能力を使って敵を攻撃してみよう！";
        }
        if (collision.gameObject.name == "EndTeaching")
        {
            tmp.text = "世界の色を取り戻すために進もう！";
        }
        if (collision.gameObject.name == "ViewTeaching")
        {
            tmp.text = "十字キーを使って周りを観察しよう！";
        }
        if (collision.gameObject.name == "UITeaching")
        {
            tmp.text = "左上で宝石のカケラの収集状況が確認できる";
        }
        if (collision.gameObject.name == "TimerTeaching")
        {
            tmp.text = "タイマーを拾って新しい能力を解放しよう";
        }

    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.name == "SkillTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "CheckPoint")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "ShardTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "JumpTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "MoveTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "SkillTeaching2")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "GuardTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "FightTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "ViewTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "UITeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "EndTeaching")
        {
            tmp.text = "";
        }
        if (col.gameObject.name == "TimerTeaching")
        {
            tmp.text = "";
        }
    }

}