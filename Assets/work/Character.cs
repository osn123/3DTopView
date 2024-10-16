using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("state")]
    [SerializeField]protected float maxHealth;

    [SerializeField]protected float currentHealth;

    [Header("muteki")]
    public bool invulnerable;

    public float invulnerableDuration;

    public UnityEvent OnHurt;
    public UnityEvent OnDie;
    private Rigidbody2D rb;

    protected virtual void OnEnable()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        if (invulnerable)
            return;

        if (currentHealth - damage > 0f)
        {
            currentHealth -= damage;
            StartCoroutine(nameof(InvulnerableCoroutine));
            //shoushang donghua
            OnHurt?.Invoke();
        }
        else
        {
            
            Die();
        }
    }

    public virtual void Die() 
    {
        currentHealth = 0f;

        OnDie?.Invoke();


        //Destroy(gameObject);
    }


    protected virtual IEnumerator InvulnerableCoroutine()
    {
        invulnerable = true;

        yield return new WaitForSeconds(invulnerableDuration);

        invulnerable = false;
    }

}
