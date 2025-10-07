using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : ProjectileMovement , IProjectile
{
    public static Bullet main;

    private float bulletDamage = 3f;
    private bool isDone;

    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (isDone)
        {
            return;
        }
        isDone = true;
        collision2D.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);
        Destroy(gameObject);
    }

    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }
}
