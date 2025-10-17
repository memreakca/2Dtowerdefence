using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    private float hitPoints;
    [SerializeField] private Image hpBar;
    [SerializeField] private GameObject hpBarCanvas;

    private EnemyAttributes enemyAttributes;
    private bool isDestroyed = false;

    private void Start()
    {
        enemyAttributes = GetComponent<EnemyAttributes>();
        hitPoints = enemyAttributes.GetMaxHp();
        hpBarCanvas.SetActive(false);
    }

    public void TakeDamage(float dmg)
    {
        hitPoints -= dmg;
        if (hitPoints < enemyAttributes.GetMaxHp() && hitPoints > 0)
        {
            hpBarCanvas.SetActive(true);
            hpBar.fillAmount = hitPoints / enemyAttributes.GetMaxHp();
        }
        else hpBarCanvas.SetActive(false);

        if (hitPoints <= 0 && !isDestroyed)
        {
            GameEvents.CurrencyGathered(enemyAttributes.GetCurrencyWorth());
            GameEvents.EnemyDied(enemyAttributes);
            isDestroyed = true;
        }
    }
}
