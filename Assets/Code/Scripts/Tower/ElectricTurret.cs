using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ElectricTurret : Turret
{
    public float shockDuration;
    protected override void SpawnProjectile()
    {
        if(target != null)
        {
            var spawnedProjectile = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
            LineRenderer lr = spawnedProjectile.GetComponent<LineRenderer>();
            lr.SetPosition(0, spawnedProjectile.transform.position);
            lr.SetPosition(1, target.position);

            var enemy = target.GetComponent<EnemyHealth>();
            if (enemy != null)
                enemy.TakeDamage(modifiedDamage);
            var enemyMov = target.GetComponent<EnemyAttributes>();
            enemyMov.ApplyShock(shockDuration);
            Destroy(spawnedProjectile,modifiedBps * 0.5f);
        }
    }
}
