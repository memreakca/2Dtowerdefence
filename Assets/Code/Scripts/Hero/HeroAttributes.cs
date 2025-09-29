using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroAttributes : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Animator animator;
    [SerializeField] public Transform basePosition;
    [SerializeField] private LayerMask enemyMask;

    [Header("UI References")]
    [SerializeField] private Image hpbarImage;
    [SerializeField] private GameObject hpBarCanvas;

    [Header("Attributes")]
    [SerializeField] protected float maxHp;
    [SerializeField] protected float Hp;
    [SerializeField] protected float attackDamage;
    [SerializeField] protected float targetingRange;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float attackCooldown;


    private Transform target;
    private EnemyMovement enemyMovement;
    private EnemyHealth enemyHealth;

    private float attackTimer;
    public bool isDead;
    private void Start()
    {
        Hp = maxHp;
    }

    private void Update()
    {
        if (isDead) return;
        attackTimer -= Time.deltaTime; // her karede geri say
        if (target == null)
        {
            FindTarget();
            ReturnToBasePosition();
            return;
        }

        if (Vector2.Distance(target.position, transform.position) <= 0.5f)
        {
            if (enemyMovement != null)
            {
                enemyMovement.targetHeroAttributes = this;
                enemyMovement.StopMoving();
            }

            // Cooldown dolduysa saldýr
            if (attackTimer <= 0f)
            {
                AttackToTarget();
                attackTimer = attackCooldown; // cooldown resetle
            }
        }
        else
        {
            MoveToTarget();
        }
    }
    public void TakeDamage(float damage)
    {
        Hp -= damage;

        if (Hp < maxHp && Hp > 0)
        {
            hpBarCanvas.SetActive(true);
            hpbarImage.fillAmount = Hp / maxHp;
        }
        else
            hpBarCanvas.SetActive(false);

        if (Hp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (enemyMovement != null)
        {
            Debug.Log("speed resetted");
            enemyMovement.ResetSpeed();
            enemyMovement.targetHeroAttributes = null;
        }

        if (!isDead)
        {
            Debug.Log("öldün");
            isDead = true;
            animator.SetTrigger("Die");
            animator.SetBool("isMoving", false);
        }
    }

    public void DestroyAnimation()
    {
        Destroy(gameObject);
    }
    public void AttackToTarget()
    {
        Debug.Log("KNÝGHT ATTACK TRÝGERRED");
        rb.velocity = Vector2.zero;
        animator.SetTrigger("Attack");
        animator.SetBool("isMoving", false);

    }
    public void DealDamageToTarget()
    {
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(attackDamage);
        }
    }

    public void ReturnToBasePosition()
    {
        Vector2 direction = (basePosition.position - transform.position).normalized;
        sr.flipX = basePosition.position.x - transform.position.x < 0;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isMoving", true);


        if (Vector2.Distance(basePosition.position, transform.position) <= 0.1f)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("isMoving", false);

        }
    }
    public void MoveToTarget()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        sr.flipX = target.position.x - transform.position.x < 0;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isMoving", true);
    }
    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(basePosition.position, targetingRange, enemyMask);
        if (hits.Length == 0) return;

        // en yakýn hedefi seç (daha stabil)
        float minDist = float.MaxValue;
        Transform closest = null;
        foreach (var c in hits)
        {
            float d = Vector2.Distance(basePosition.position, c.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = c.transform;
            }
        }

        if (closest != null)
        {
            target = closest;
            enemyMovement = target.GetComponent<EnemyMovement>();
            enemyHealth = target.GetComponent<EnemyHealth>();
        }
    }

}
