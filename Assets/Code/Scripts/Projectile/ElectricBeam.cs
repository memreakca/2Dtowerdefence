using UnityEngine;

public class ElectricBeam : MonoBehaviour, IProjectile
{
    private Transform target;
    private LineRenderer lr;
    private float damage;
    private float duration = 0.05f; // anlýk dalga, çok kýsa süre
    private float timer;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetProjectileSpeed(float speed) { } // boþ, elektrik hýzla gider

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.enabled = false;
    }

    private void OnEnable()
    {
        lr.enabled = true;
        timer = 0;
    }

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Baþlangýç ve bitiþ noktalarýný sürekli güncelle
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, target.position);

        // Hedefe hasar uygula
        var enemy = target.GetComponent<EnemyHealth>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        // Kýsa sürede kaybol
        timer += Time.deltaTime;
        if (timer >= duration)
            Destroy(gameObject);
    }
}
