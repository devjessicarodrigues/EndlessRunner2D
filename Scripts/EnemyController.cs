using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public Rigidbody2D enemyRb;
    private bool faceFlip;

    private Animator animator;
    private Collider2D enemyCollider;
    private Rigidbody2D rb;

    private bool isSitting = false; 

    void Start()
    {
        animator = GetComponent<Animator>();
        enemyCollider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (!isSitting)
        {
            transform.Translate(Vector2.left * speed * Time.deltaTime);
        }
    }

    private void FlipEnemy()
    {
        if (faceFlip)
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerSit();
        }
        else if (collision != null && !collision.collider.CompareTag("Normal"))
        {
            faceFlip = !faceFlip;
            FlipEnemy();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Egg"))
        {
            TriggerSit();
        }
    }

    private void TriggerSit()
    {
        animator.SetTrigger("Sit");
        isSitting = true; 

        if (enemyCollider != null)
        {
            enemyCollider.enabled = false;
        }

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
        }
        speed = 0f;
        Destroy(gameObject, 1f);  
    }
}
