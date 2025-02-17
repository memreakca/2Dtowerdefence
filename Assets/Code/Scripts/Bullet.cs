using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet main;
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 2.5f;
    [SerializeField] private float bulletDamage = 3f;
    [SerializeField] private float maxBulletdamage = 7f;
    [SerializeField] private float UpgradeCost = 5000f;

    private bool isDone;
    public static float extraDamage;
    
    private Transform target;
  

    public void UpgradeBulletDamage()
    {
        if (extraDamage + bulletDamage == maxBulletdamage)
            return;// Debug.Log(" MAX bullet Damage");
        else
        {
            if (LevelManager.main.currency >= UpgradeCost)
            {
                LevelManager.main.currency = LevelManager.main.currency - UpgradeCost;
                extraDamage++;

            }
            else
                return;//Debug.Log("Not Enogh Money");
        }
        
    }
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

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 85f;
        // Apply rotation to the bullet sprite
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        rb.velocity = direction * bulletSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision2D)
    {//take hp from enemy
        if (isDone)
        {
            return;
        }
        isDone = true;
        collision2D.gameObject.GetComponent<Health>().TakeDamage(extraDamage+bulletDamage);
        Destroy(gameObject);
    }
}
