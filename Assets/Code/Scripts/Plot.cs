using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;

    private GameObject towerObject;
    public Turret turret;
  

    private void OnMouseDown()
    {
        if (towerObject != null) {
            
            return; }

        Tower towerToBuild = BuildManager.main.GetSelectedTower();
        if (towerToBuild.cost > LevelManager.main.currency)
        {
            Debug.Log("You cant afford this tower");
            return;

        }
        LevelManager.main.SpendCurrency(towerToBuild.cost); 
        towerObject = Instantiate(towerToBuild.prefab, transform.position, Quaternion.identity);
        turret = towerObject.GetComponent<Turret>();    
    }
}
