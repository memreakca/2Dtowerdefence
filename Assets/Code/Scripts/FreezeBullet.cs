using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float bulletDamage = 1.5f;
    [SerializeField] private float FreezeTime = 1f;
    [SerializeField] private float slowFactor = 0.5f;

    private bool isDone;
    private Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        if (!target)
        {
            return;
        }
        Vector2 direction = (target.position - transform.position).normalized;
        //Bullet rotation to enemy
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = direction * bulletSpeed;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {//take hp from enemy

        if (isDone)
        {
            return;
        }
        isDone = true;

        other.gameObject.GetComponent<Health>().TakeDamage(bulletDamage);

        Destroy(gameObject);

        EnemyMovement enemy = other.gameObject.GetComponent<EnemyMovement>();
        enemy.ApplySlow(slowFactor, FreezeTime);
        
    }

    
}
