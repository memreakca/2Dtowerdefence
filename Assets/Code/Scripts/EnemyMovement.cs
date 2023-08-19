using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public static EnemyMovement main;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer sr;
   
    [Header("Attiributes")]
    [SerializeField] private float moveSpeed = 2f;

    private int damage = 1;
    private bool isSlowed = false;
    private float baseSpeed;
    private Transform target;
    private int pathIndex = 0;
   

    private void Start()
    {
        baseSpeed = moveSpeed;
        target = LevelManager.main.path[pathIndex];
    }
  
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathIndex++;
            

            if (pathIndex == LevelManager.main.path.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;
            }
            else { 
               target = LevelManager.main.path[pathIndex];
                if(target.position.x - transform.position.x >= 0)
                {
                    sr.flipX = false;
                }
                else
                {
                    sr.flipX = true;
                }
                   
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;

        rb.velocity = direction * moveSpeed;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            moveSpeed *= slowFactor;

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
    private System.Collections.IEnumerator ResetSpeedAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);


        moveSpeed = baseSpeed;
        isSlowed = false;
    }

}
