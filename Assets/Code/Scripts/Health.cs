using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private float hitPoints = 4;
    [SerializeField] private int currencyWorth = 50;

    private bool isDestroyed = false;

    public void TakeDamage(float dmg)
    {
        hitPoints -= dmg;

        if(hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            LevelManager.main.IncreaseCurrency(currencyWorth);
            isDestroyed = true;
            Destroy(gameObject);
        }
    }
}
