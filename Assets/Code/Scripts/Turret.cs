using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.Mathematics;
using UnityEngine.UI;
using TMPro;
using System.Data;

public class Turret : MonoBehaviour
{
    public static Turret main;

    [Header("References")]
    [SerializeField] private GameObject Tower;
    [SerializeField] TextMeshProUGUI UpgradeCostUI;
    [SerializeField] TextMeshProUGUI SellCostUI;
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Animator animator;

    [Header("UpgradeTurretMultipliers")]
    [SerializeField] private float bpsUpgradeFactor;
    [SerializeField] private float rangeUpgradeFactor;
    [SerializeField] private float costUpgradeFactor;

    [Header("Attribute")]
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 1f; // Bullet per sec
    [SerializeField] private float baseBps = 1f; // Basebps per sec
    [SerializeField] public float UpgradeCost = 150;
    [SerializeField] private int TurretLevel = 1;
    [SerializeField] private int maxTurretLevel = 4;
    [SerializeField] private bool isRotateable = false;

    public float sellCost;


    public void UpgradeTurret()
    {
        if (TurretLevel == maxTurretLevel)
            return; //Debug.Log("It is MAX Level");
        else
        {
            
            if (LevelManager.main.currency >= UpgradeCost)
            {
                LevelManager.main.currency = LevelManager.main.currency - UpgradeCost;
                bps = bps * bpsUpgradeFactor;
                targetingRange = targetingRange * rangeUpgradeFactor;
                UpgradeCost = UpgradeCost * costUpgradeFactor;
                TurretLevel++;

            }
            else return;/*Debug.Log("You Cant Afford This Upgrade");*/
        }
    }

    private Transform target;
    private float timeUntilFire;

    public void sellTurret()
    {
        sellCost = UpgradeCost * 0.4f;
        LevelManager.main.currency += sellCost;
        Destroy(Tower);
    }
    private void UpdateUpgradeUI()
    {

        if (TurretLevel == maxTurretLevel)
            UpgradeCostUI.text = "MAX LEVEL";
        else
            UpgradeCostUI.text ="Upgrade Cost= " + UpgradeCost.ToString() + "$" + "\nTurret Level= " + TurretLevel.ToString();
    }

    private void UpdateSellCostUI()
    {
        SellCostUI.text = "Sell Turret " + (UpgradeCost * 0.4).ToString() + "$" ;
    }
    void Update()
    {
        UpdateUpgradeUI();
        UpdateSellCostUI();

        animator.speed = bps / baseBps;

        if (Input.GetKeyDown(KeyCode.K))
        {
            bps = bps * 1.15f;
            targetingRange = targetingRange * 1.1f;
            TurretLevel++;
        }

        if (target == null)
        { FindTarget();
            return;
        }

        RotateTowardsTarget();

        if (!CheckTargetIsInRange())
        {
            target = null;
        }
        else
        {
            timeUntilFire += Time.deltaTime;
            if(timeUntilFire >= 1f / bps)
            {
                Shoot();
                timeUntilFire = 0f;
            }
        }
    }

    
     private void Shoot()
    {
        animator.SetTrigger("Shoot");
 
    }

    public void SpawnProjectile()
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        ProjectileMovement projectileScript = bulletObj.GetComponent<ProjectileMovement>();
        projectileScript.SetTarget(target);
    }
   
    private void RotateTowardsTarget()
    {
        if (isRotateable)
        {
            Vector3 pivot = turretRotationPoint.position;
            Vector2 dir = (target.position - pivot);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset; // angleOffset ile sprite yönünü düzelt
            Quaternion desired = Quaternion.Euler(0f, 0f, angle);
            turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, desired, rotationSpeed * Time.deltaTime);

            Debug.DrawLine(pivot, target.position, Color.red);
            Debug.DrawRay(pivot, turretRotationPoint.right * 1f, Color.green);
        }
    }


    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, turretRotationPoint.position) <= targetingRange;
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(turretRotationPoint.position, targetingRange, enemyMask);
        if (hits.Length == 0) return;

        // en yakýn hedefi seç (daha stabil)
        float minDist = float.MaxValue;
        Transform closest = null;
        foreach (var c in hits)
        {
            float d = Vector2.Distance(turretRotationPoint.position, c.transform.position);
            if (d < minDist)
            {
                minDist = d;
                closest = c.transform;
            }
        }
        target = closest;
    }

}
