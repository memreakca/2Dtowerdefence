using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class FreezeTurret : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI UpgradeCostUI;
    [SerializeField] TextMeshProUGUI SellCostUI;
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;

    [Header("Attribute")]
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float bps = 0.5f; // Bullet per sec
    [SerializeField] public int UpgradeCost = 200;
    [SerializeField] private int TurretLevel = 1;
    [SerializeField] private int maxTurretLevel = 4;

    public float sellCost;


    private Transform target;
    private float timeUntilFire;
  
    public void UpgradeTurret()
    {
        if (TurretLevel == maxTurretLevel)
            Debug.Log("It is MAX Level");
        else
        {
            if (LevelManager.main.currency >= UpgradeCost)
            {
                LevelManager.main.currency = LevelManager.main.currency - UpgradeCost;
                bps = bps * 1.2f;
                targetingRange = targetingRange * 1.2f;
                UpgradeCost = UpgradeCost * 2;
                TurretLevel++;

            }
            else Debug.Log("You Cant Afford This Upgrade");
        }
    }
    private void UpdateUpgradeUI()
    {

        if (TurretLevel == maxTurretLevel)
            UpgradeCostUI.text = "MAX LEVEL";
        else
            UpgradeCostUI.text = "Upgrade Cost= " + UpgradeCost.ToString() + "\nTurret Level= " + TurretLevel.ToString();
    }

    private void UpdateSellCostUI()
    {
        SellCostUI.text = "Sell Turret" ;
    }
    void Update()
    {
        UpdateUpgradeUI();
        UpdateSellCostUI();

        if (target == null)
        {
            FindTarget();
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
            if (timeUntilFire >= 1f / bps)
            {
                Shoot1();
                timeUntilFire = 0f;
            }
        }
    }
    public void sellTurret()
    {
        sellCost = UpgradeCost * 0.4f;
        LevelManager.main.currency += sellCost;
        Destroy(gameObject);
    }


    private void Shoot1()
    {
        GameObject bulletObj1 = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        FreezeBullet bulletScript1 = bulletObj1.GetComponent<FreezeBullet>();
        bulletScript1.SetTarget(target);

    }
    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, transform.position) <= targetingRange;
    }
    private void RotateTowardsTarget()
    {
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - transform.position.x) * Mathf.Rad2Deg - 130f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

  
    private void FindTarget()

    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingRange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0)
        {
            target = hits[0].transform;
        }
    }

    void Start()
    {

    }
}
