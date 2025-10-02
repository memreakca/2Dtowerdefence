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
    [SerializeField] public Vector3 basePosition;
    [SerializeField] private LayerMask enemyMask;

    [Header("UI References")]
    [SerializeField] private Image hpbarImage;
    [SerializeField] private GameObject hpBarCanvas;

    [Header("Attributes")]
    [SerializeField] public float maxHp;
    [SerializeField] public float Hp;
    [SerializeField] public float attackDamage;
    [SerializeField] public float targetingRange;
    [SerializeField] public float moveSpeed;
    [SerializeField] public float attackCooldown;


    private Transform target;
    private EnemyMovement enemyMovement;
    private EnemyHealth enemyHealth;

    private HeroTower tower;
    private Vector3 offset;

    private float attackTimer;
    public bool isDead;
    private void Start()
    {
        Hp = maxHp;
    }
    public void Init(HeroTower t)
    {
        tower = t;
        offset = transform.position - tower.transform.position; 
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

        if (Vector2.Distance(target.position, transform.position) <= 0.3f)
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
            tower.targetedEnemies.Remove(enemyMovement);
            enemyMovement.ResetSpeed();
            enemyMovement.targetHeroAttributes = null;
        }

        if (!isDead)
        {
            isDead = true;
            animator.SetTrigger("Die");
            animator.SetBool("isMoving", false);
            float dieAnimLength = animator.GetCurrentAnimatorStateInfo(0).length - 0.05f;
            tower.QueueHeroForRespawn(this);
            target = null;
            Invoke("DisableGameObject", dieAnimLength);
        }
    }
    private void OnDisable()
    {
        if (enemyMovement != null)
            tower.targetedEnemies.Remove(enemyMovement);
    }
    public void DisbleGameObject()
    {
        gameObject.SetActive(false);
    }
    public void AttackToTarget()
    {
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
        Vector2 direction = (basePosition - transform.position).normalized;
        sr.flipX = basePosition.x - transform.position.x < 0;
        rb.velocity = direction * moveSpeed;
        animator.SetBool("isMoving", true);


        if (Vector2.Distance(basePosition, transform.position) <= 0.1f)
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
        Collider2D[] hits = Physics2D.OverlapCircleAll(basePosition, targetingRange, enemyMask);
        if (hits.Length == 0) return;

        // en yakýn hedefi seç (daha stabil)
        float minDist = float.MaxValue;
        Transform closest = null;
        foreach (var c in hits)
        {
            var enemy = c.GetComponent<EnemyMovement>();
            if (enemy == null || tower.targetedEnemies.Contains(enemy) || enemy.isDead) continue;

            float d = Vector2.Distance(basePosition, c.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = c.transform;
            }
        }


        if (closest != null )
        {
            target = closest;
            enemyMovement = target.GetComponent<EnemyMovement>();
            enemyHealth = target.GetComponent<EnemyHealth>();
            tower.targetedEnemies.Add(enemyMovement);
        }
    }

}
