using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public static EnemyMovement main;
    [Header("References")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] protected SpriteRenderer sr;
    [SerializeField] protected Animator animator;

    [Header("Attiributes")]
    [SerializeField] protected float moveSpeed = 2f;
    [SerializeField] protected float enemyDamage;
    [SerializeField] private float attackCooldown = 1f;

    private float attackTimer = 0f;

    private int damage = 1;
    private bool isSlowed = false;
    protected float baseSpeed;
    private Transform target;
    public int pathIndex = 0;
    private Color originalColor;
    public bool isFighting;
    public bool isDead;

    public HeroAttributes targetHeroAttributes;



    private void Start()
    {
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
                EnemySpawner.onEnemyDestroy.Invoke();
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
        Debug.Log("ENEMY ATTACK TRIGERRED");
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
        if (other.gameObject.TryGetComponent<Base>(out var basee))
        {

            basee.TakeDamage(damage);
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

    public void Die()
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
