using UnityEngine;

public class ElectricBeam : MonoBehaviour, IProjectile
{
    private Transform target;
    private LineRenderer lr;
    private float damage;
    private float duration = 0.05f; // anl�k dalga, �ok k�sa s�re
    private float timer;

    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void SetProjectileSpeed(float speed) { } // bo�, elektrik h�zla gider

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

        // Ba�lang�� ve biti� noktalar�n� s�rekli g�ncelle
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, target.position);

        // Hedefe hasar uygula
        var enemy = target.GetComponent<EnemyHealth>();
        if (enemy != null)
            enemy.TakeDamage(damage);

        // K�sa s�rede kaybol
        timer += Time.deltaTime;
        if (timer >= duration)
            Destroy(gameObject);
    }
}
