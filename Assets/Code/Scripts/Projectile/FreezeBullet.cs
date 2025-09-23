using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeBullet : MonoBehaviour
{

    [Header("Attributes")]
    [SerializeField] private float bulletDamage = 1.5f;
    [SerializeField] private float FreezeTime = 1f;
    [SerializeField] private float slowFactor = 0.5f;

    private bool isDone;
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
