using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{
    public static EnemyAttributes main;
    [SerializeField] private EnemyObject enemySO;

    [Header("Runtime")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator animator;

    protected float moveSpeed;
    protected float enemyDamage;
    protected float attackCooldown;
    protected int damageToBase;
    protected int currencyWorth;
    protected float maxHp;
    public int GetCurrencyWorth() => currencyWorth;
    public float GetMaxHp() => maxHp;

    private float attackTimer = 0f;

    private bool isSlowed = false;
    protected float baseSpeed;
    private Transform target;
    public int pathIndex = 0;
    private Color originalColor;
    public bool isFighting;
    public bool isDead;

    public HeroAttributes targetHeroAttributes;

    private void OnEnable()
    {
        GameEvents.OnEnemyDie += Die;
    }
    private void OnDisable()
    {
        GameEvents.OnEnemyDie -= Die;
    }
    private void Start()
    {
        moveSpeed = enemySO.moveSpeed;
        enemyDamage = enemySO.enemyDamage;
        attackCooldown = enemySO.attackCooldown;
        damageToBase = enemySO.damageToBase;
        currencyWorth = enemySO.currencyWorth;
        maxHp = enemySO.maxHp;

        baseSpeed = moveSpeed;
        target = PathManager.main.path[pathIndex];
        originalColor = GetComponent<SpriteRenderer>().color;
        OnStartFunction();
        animator.SetBool("isMoving", true);
    }

    private void Update()
    {
        if (isDead) return;

        if (target == null) return;

        if (targetHeroAttributes != null)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer < 0f)
            {

                Attack();
                attackTimer = attackCooldown;
            }
        }

        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;

            if (pathIndex == PathManager.main.path.Length)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                target = PathManager.main.path[pathIndex];
                sr.flipX = target.position.x - transform.position.x < 0;

                OnPathIndexChanged();
            }
        }

    }
    protected virtual void OnPathIndexChanged()
    {
        // Normal düşman için boş
    }
    protected virtual void OnStartFunction()
    {
        // Normal düşman için boş
    }

    public void Attack()
    {
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Attack");
    }
    public void DealDamageToHero()
    {
        if (!isDead && targetHeroAttributes != null)
        {
            targetHeroAttributes.TakeDamage(enemyDamage);
        }
    }
    public void StopMoving()
    {
        moveSpeed = 0;
        isFighting = true;
        animator.SetBool("isMoving", false);
    }
    private void FixedUpdate()
    {
        if (isDead) return;
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            moveSpeed *= slowFactor;
            GetComponent<SpriteRenderer>().color = Color.Lerp(originalColor, Color.blue, 0.3f);
            // Reset the speed after the duration
            StartCoroutine(ResetSpeedAfterDelay(duration));
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Base"))
        {
            GameEvents.EnemyEnteredBase(damageToBase,this);
            Destroy(gameObject);
        }
        else return;

    }
    private IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        GetComponent<SpriteRenderer>().color = originalColor;
        ResetSpeed();
        isSlowed = false;
    }

    public void ResetSpeed()
    {
        animator.SetBool("isMoving", true);
        moveSpeed = baseSpeed;
        isFighting = false;
    }

    public void Die(EnemyAttributes enemy)
    {
        if (enemy == this)
        {
            isDead = true;
            animator.SetBool("isMoving", false);
            animator.SetTrigger("Die");
            rb.velocity = Vector2.zero;
            float dieAnimLength = animator.GetCurrentAnimatorStateInfo(0).length - 0.05f;
            Destroy(gameObject, dieAnimLength);
            this.enabled = false;
        }
    }

}
