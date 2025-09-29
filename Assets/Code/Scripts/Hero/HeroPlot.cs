using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPlot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform HeroSpawnPoint;


    private GameObject heroObject;


    private void OnMouseDown()
    {
        if (heroObject != null)
        {
            return;
        }

        Hero heroToSpawn = BuildManager.main.GetSelectedHero();
        if (heroToSpawn.cost > LevelManager.main.currency)
        {
            Debug.Log("You cant afford this tower");
            return;

        }
        LevelManager.main.SpendCurrency(heroToSpawn.cost);
        heroObject = Instantiate(heroToSpawn.prefab, HeroSpawnPoint.position, Quaternion.identity);
        heroObject.transform.SetParent(this.transform);
        sr.enabled = false;
    }
}
