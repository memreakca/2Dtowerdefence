using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [Header("Attributes")]
    private float hitPoints = 4;
    [SerializeField] private float maxHp = 4;
    [SerializeField] private int currencyWorth = 50;
    [SerializeField] private float hpMultiplier;
    [SerializeField] Image hpBar;
    [SerializeField] GameObject hpBarCanvas;

    private bool isDestroyed = false;

    private void Start()
    {
        maxHp = maxHp + (hpMultiplier * EnemySpawner.main.currentWave);
        hitPoints = maxHp;
        hpBarCanvas.SetActive(false);

    }
    public void TakeDamage(float dmg)
    {
        hitPoints -= dmg;

        if (hitPoints < maxHp && hitPoints > 0)
        {
            hpBarCanvas.SetActive(true);
            hpBar.fillAmount = hitPoints / maxHp;
        }
        else
            hpBarCanvas.SetActive(false);


        if (hitPoints <= 0 && !isDestroyed)
        {
            EnemySpawner.onEnemyDestroy.Invoke();
            GameEvents.CurrencyGathered(currencyWorth);
            GameEvents.EnemyDied(GetComponent<EnemyAttributes>());
            isDestroyed = true;
        }
    }

}
