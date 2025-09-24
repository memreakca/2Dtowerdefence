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
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private Animator animator;

    [Header("UpgradeTurretMultipliers")]
    [SerializeField] private float bpsUpgradeFactor;
    [SerializeField] private float rangeUpgradeFactor;
    [SerializeField] private float costUpgradeFactor;
    [SerializeField] public float UpgradeCost;

    [Header("General Attributes")]
    [SerializeField] private bool isRotateable = false;
    [SerializeField] private float angleOffset = 0f;
    [SerializeField] private float rotationSpeed = 5f;

    [Header("Turret Attributes")]
    [SerializeField] private int maxTurretLevel;
    [SerializeField] private float baseBps;
    [SerializeField] private float baseTargetingRange;
    [SerializeField] private float baseProjectileDamage;
    [SerializeField] private float baseProjectileSpeed;


    [Header("IN-GAME Base Attributes")]
    [SerializeField] private float projectileDamage;
    [SerializeField] private float targetingRange = 5f;
    [SerializeField] private float bps = 1f; // Bullet per sec
    [SerializeField] private int TurretLevel = 1;

    [Header("IN-GAME Bonus Attributes")]

    [SerializeField] private float bonusDamage;
    [SerializeField] private float bonusRange;
    [SerializeField] private float bonusBps;

    [Header("IN-GAME Modifed - Used Attributes")]

    [SerializeField] private float modifiedDamage;
    [SerializeField] private float modifiedRange;
    [SerializeField] private float modifiedBps;

    public float sellCost;
    private void Start()
    {
        projectileDamage = baseProjectileDamage;
        modifiedDamage = projectileDamage + bonusDamage;

        bps = baseBps;
        modifiedBps = bps + bonusBps;

        targetingRange = baseTargetingRange;
        modifiedRange = targetingRange+bonusRange;
        UpgradedValues();
    }

    public void UpgradedValues()
    {
        Debug.Log("bps VALUE " + bonusBps);

        bonusBps = UpgradeManager.instance.bpsUpgradeValue;
        bonusRange = UpgradeManager.instance.rangeUpgradeValue;
        bonusDamage = UpgradeManager.instance.damageUpgradeValue;

        modifiedDamage = projectileDamage + bonusDamage;
        modifiedBps = bps + bonusBps;
        modifiedRange = targetingRange + bonusRange;
    }

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
            UpgradeCostUI.text = "Upgrade Cost= " + UpgradeCost.ToString() + "$" + "\nTurret Level= " + TurretLevel.ToString();
    }

    private void UpdateSellCostUI()
    {
        SellCostUI.text = "Sell Turret " + (UpgradeCost * 0.4).ToString() + "$";
    }
    void Update()
    {
        UpdateUpgradeUI();
        UpdateSellCostUI();
        
        animator.speed = modifiedBps / baseBps;

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
            if (timeUntilFire >= 1f / modifiedBps)
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
        GameObject projectileObj = Instantiate(projectilePrefab, firingPoint.position, Quaternion.identity);
        IProjectile damageScript = projectileObj.GetComponent<IProjectile>();
        damageScript.SetDamage(modifiedDamage);
        ProjectileMovement projectileScript = projectileObj.GetComponent<ProjectileMovement>();
        projectileScript.SetProjectileSpeed(baseProjectileSpeed);
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
        return Vector2.Distance(target.position, turretRotationPoint.position) <= (modifiedRange);
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(turretRotationPoint.position, (modifiedRange), enemyMask);
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
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, (modifiedRange));
    }
}
