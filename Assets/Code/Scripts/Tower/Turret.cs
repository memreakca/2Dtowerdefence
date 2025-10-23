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
    [SerializeField] private Transform towerRotationPoint;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] protected GameObject projectilePrefab;
    [SerializeField] protected Transform firingPoint;
    [SerializeField] private Animator animator;

    [Header("UpgradeTowerMultipliers")]
    [SerializeField] private float bpsUpgradeFactor;
    [SerializeField] private float rangeUpgradeFactor;
    [SerializeField] private float costUpgradeFactor;
    [SerializeField] public float UpgradeCost;

    [Header("General Attributes")]
    [SerializeField] private bool isRotateable = false;
    [SerializeField] private float angleOffset;
    [SerializeField] private float rotationSpeed;

    [Header("Tower Attributes")]
    [SerializeField] private int maxTowerLevel;
    [SerializeField] private float baseBps;
    [SerializeField] private float baseTargetingRange;
    [SerializeField] private float baseProjectileDamage;
    [SerializeField] private float baseProjectileSpeed;


    [Header("IN-GAME Base Attributes")]
    [SerializeField] private float projectileDamage;
    [SerializeField] private float targetingRange;
    [SerializeField] private float bps; 
    [SerializeField] private int TowerLevel;

    [Header("IN-GAME Bonus Attributes")]

    [SerializeField] private float bonusDamage;
    [SerializeField] private float bonusRange;
    [SerializeField] private float bonusBps;

    [Header("IN-GAME Modifed - Used Attributes")]

    [SerializeField] public float modifiedDamage;
    [SerializeField] public float modifiedRange;
    [SerializeField] public float modifiedBps;

    [Header("HideInInspector")]
    [HideInInspector] public float sellCost;
    [HideInInspector] public Transform target;
    [HideInInspector] public float timeUntilFire;
    private void Start()
    {
        projectileDamage = baseProjectileDamage;
        modifiedDamage = projectileDamage + bonusDamage;

        bps = baseBps;
        modifiedBps = bps + bonusBps;

        targetingRange = baseTargetingRange;
        modifiedRange = targetingRange+bonusRange;
        UpdateUI();
        ChangeUpgradedValues();
    }

    public void ChangeUpgradedValues()
    {

        bonusBps = UserManager.Instance.bonusBps;       
        bonusRange = UserManager.Instance.bonusRange;
        bonusDamage = UserManager.Instance.bonusDamage;

        modifiedDamage = projectileDamage + bonusDamage;
        modifiedBps = bps + bonusBps;
        modifiedRange = targetingRange + bonusRange;
    }

    public void UpgradeTower()
    {
        if (TowerLevel == maxTowerLevel)
            return; //Debug.Log("It is MAX Level");
        else
        {

            if (GameStateManager.Instance.currency >= UpgradeCost)
            {
                GameEvents.CurrencySpend(UpgradeCost);
                bps = bps * bpsUpgradeFactor;
                targetingRange = targetingRange * rangeUpgradeFactor;
                UpgradeCost = UpgradeCost * costUpgradeFactor;
                TowerLevel++;
                ChangeUpgradedValues();
                UpdateUI();
            }
            else return;/*Debug.Log("You Cant Afford This Upgrade");*/
        }
    }


    public void SellTower()
    {
        sellCost = UpgradeCost * 0.4f;
        GameEvents.CurrencyGathered(sellCost);
        Destroy(Tower);
    }


    private void UpdateUI()
    {

        if (TowerLevel == maxTowerLevel)
            UpgradeCostUI.text = "MAX LEVEL";
        else
            UpgradeCostUI.text = "Upgrade Cost= " + UpgradeCost.ToString() + "<sprite index= 0>" + "\nTower Level= " + TowerLevel.ToString();

        SellCostUI.text = "Sell Tower " + (UpgradeCost * 0.4).ToString() + "<sprite index= 0>";
    }
    public void Update()
    {

        animator.speed = Mathf.Max(1f, modifiedBps / baseBps);

        if (target == null || target.GetComponent<EnemyAttributes>().isDead)
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

    protected virtual void SpawnProjectile()
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
            Vector3 pivot = towerRotationPoint.position;
            Vector2 dir = (target.position - pivot);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + angleOffset; // angleOffset ile sprite yönünü düzelt
            Quaternion desired = Quaternion.Euler(0f, 0f, angle);
            towerRotationPoint.rotation = Quaternion.RotateTowards(towerRotationPoint.rotation, desired, rotationSpeed * Time.deltaTime);

            Debug.DrawLine(pivot, target.position, Color.red);
            Debug.DrawRay(pivot, towerRotationPoint.right * 1f, Color.green);
        }
    }


    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(target.position, towerRotationPoint.position) <= (modifiedRange);
    }

    private void FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(towerRotationPoint.position, (modifiedRange), enemyMask);
        if (hits.Length == 0) return;

        // en yakýn hedefi seç (daha stabil)
        float minDist = float.MaxValue;
        Transform closest = null;
        foreach (var c in hits)
        {
            float d = Vector2.Distance(towerRotationPoint.position, c.transform.position);
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
