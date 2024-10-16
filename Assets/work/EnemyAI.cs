using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : Character
{
    private Animator anim;
    public Transform player;
    public float detectionRange = 3f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float attackCooldown = 2f;
    public float attackDuration = 0.5f;

    private bool isAttacking = false;
    private bool canAttack = true;

    Transform AttackTf;

    private PlayerController playerScript;
    private Rigidbody2D rb;

    GameObject facingLeft;

    public float damage;
    private bool bulletflip;
    private bool canBulletFlip;
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();

        AttackTf = transform.Find("AttackTf");

        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                playerScript = playerObject.GetComponent<PlayerController>();
            }
        }
    }

    void Update()
    {
        SearchPlayer();
        bool isMoving = rb.velocity.x != 0;
        //anim.SetBool("isMoving", isMoving);
        anim.SetBool("isAttacking", isAttacking);
        EnemyDie();
    }

    private void SearchPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (!isAttacking && canAttack)
            {
                StartCoroutine(AttackPlayer());
            }
        }

        if (distanceToPlayer >= detectionRange) 
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsPlayer()     
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction.x < 0.0f)
        {
            transform.eulerAngles = Vector3.zero; 
        }
        else if (direction.x > 0.0f)
        {
            transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);
        }
    }
   
    private IEnumerator AttackPlayer()
    {
        isAttacking = true;
        canAttack = false;
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        //Debug.Log("Enemy attack cooldown finished");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Character>().TakeDamage(damage);
        }
    }

    public void CreateBullet()
    {
        GameObject obj = Instantiate(Resources.Load("Bullet")) as GameObject;
        obj.transform.position = AttackTf.position;
        obj.transform.rotation = AttackTf.rotation;
        obj.AddComponent<Bullet>();
    }
    public void EnemyDie()
    {
        if(currentHealth==0)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            gameObject.GetComponent<Animator>().enabled = false;
            foreach (Character script in GetComponents<Character>())
            {
                script.enabled = false;
            }
        }
 
    }
}


