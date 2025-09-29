using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : MonoBehaviour , IProjectile
{

    [Header("Attributes")]
    [SerializeField] private float FreezeTime = 1f;
    [SerializeField] private float slowFactor = 0.5f;

    private float bulletDamage;
    private bool isDone;

    public void SetDamage(float damage)
    {
        bulletDamage = damage;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (isDone)
        {
            return;
        }
        isDone = true;

        other.gameObject.GetComponent<EnemyHealth>().TakeDamage(bulletDamage);

        Destroy(gameObject);

        EnemyMovement enemy = other.gameObject.GetComponent<EnemyMovement>();
        enemy.ApplySlow(slowFactor, FreezeTime);
        
    }


    
}
