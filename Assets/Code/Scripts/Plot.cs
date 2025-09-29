using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Transform towerSpawnPoint;


    private GameObject towerObject;
    public Turret turret;


    private void OnMouseDown()
    {
        if (towerObject != null)
        {
            return;
        }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You cant afford this tower");
            return;

        }
        LevelManager.main.SpendCurrency(towerToBuild.cost);
        towerObject = Instantiate(towerToBuild.prefab, towerSpawnPoint.position, Quaternion.identity);
        towerObject.transform.SetParent(this.transform);
        turret = towerObject.GetComponentInChildren<Turret>();
        BuildManager.main.builtTurrets.Add(turret);
    }
}
