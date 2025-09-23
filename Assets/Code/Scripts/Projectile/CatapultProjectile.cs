using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatapultProjectile : MonoBehaviour , IProjectile
{
    [Header("Attributes")]
    [SerializeField] private float explosionDamage = 3f;
    [SerializeField] private float explosionRadius = 10;

    [Header("Explosion Prefab")]
    [SerializeField] private GameObject explosionPrefab;

    private bool isDone;
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (isDone)
        {
            return;
        }
        isDone = true;
        Explode();
        Destroy(gameObject);
    }

    public void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position , Quaternion.identity );
        explosion.transform.localScale = new Vector3((3.333f * explosionRadius),(3.333f * explosionRadius));
        Destroy(explosion, 0.25f);

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent<Health>(out var health))
            {
                health.TakeDamage(explosionDamage);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    public void SetDamage(float damage)
    {
        explosionDamage = damage;
    }

}
