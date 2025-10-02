using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class HeroTower : MonoBehaviour
{
    public static HeroTower main;

    [Header("References")]
    [SerializeField] private GameObject Tower;
    [SerializeField] TextMeshProUGUI UpgradeCostUI;
    [SerializeField] TextMeshProUGUI SellCostUI;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private GameObject heroPrefab;
    [SerializeField] private GameObject heroSpawnPointFlag;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int segments = 50;

    [Header("Upgrade Tower Multipliers")]
    [SerializeField] private float heroDamageUpgradeFactor;
    [SerializeField] private int heroNumberUpgradeFactor;
    [SerializeField] private float heroAttackSpeedUpgradeFactor;
    [SerializeField] private float heroHpUpgradeFactor;
    [SerializeField] private float costUpgradeFactor;
    [SerializeField] public float UpgradeCost;

    [Header("Tower Attributes")]
    [SerializeField] private int maxTowerLevel;
    [SerializeField] private int baseMaxHeroNumber;
    [SerializeField] private float baseHeroHp;
    [SerializeField] private float baseHeroDamage;
    [SerializeField] private float baseHeroAttackSpeed;
    [SerializeField] private float baseHeroRespawnCooldown;
    [SerializeField] private float heroSpawnRange = 3f; 
    [SerializeField] private float heroSpawnRadius = 0.2f; 


    [Header("IN-GAME Attributes")]
    private float heroDamage;
    private int maxHeroNumber;
    private float heroAttackSpeed;
    private float heroHp;
    private int TowerLevel = 1;
    private float heroRespawnCooldown;

    [Header("IN-GAME Bonus Attributes")]
    private float bonusHeroDamage;
    private int bonusHeroNumber;
    private float bonusHeroAttackSpeed;
    private float bonusHeroHp;

    [Header("IN-GAME Modified - Used Attributes")]
    private float modifiedHeroDamage;
    private int modifiedHeroNumber;
    private float modifiedHeroAttackSpeed;
    private float modifiedHeroHp;

    public float sellCost;
    private bool flagPlaceMode;
    private Tilemap pathTilemap;
    private GameObject flagObject;

    private Transform currentSpawnPoint;
    private List<GameObject> spawnedHeroes = new List<GameObject>();
    public  HashSet<EnemyMovement> targetedEnemies = new HashSet<EnemyMovement>();

    private void Start()
    {
        pathTilemap = LevelManager.main.pathTileMap;
        heroDamage = baseHeroDamage;
        modifiedHeroDamage = heroDamage + bonusHeroDamage;

        maxHeroNumber = baseMaxHeroNumber;
        modifiedHeroNumber = maxHeroNumber + bonusHeroNumber;

        heroAttackSpeed = baseHeroAttackSpeed;
        modifiedHeroAttackSpeed = heroAttackSpeed + bonusHeroAttackSpeed;

        heroHp = baseHeroHp;
        modifiedHeroHp = heroHp + bonusHeroHp;

        heroRespawnCooldown = baseHeroRespawnCooldown;

        ChangeUpgradedValues();
        UpdateUI();
    }

    public void ChangeUpgradedValues()
    {
        bonusHeroAttackSpeed = UpgradeManager.instance.bpsUpgradeValue;
        bonusHeroDamage = UpgradeManager.instance.damageUpgradeValue;

        modifiedHeroDamage = heroDamage + bonusHeroDamage;
        modifiedHeroAttackSpeed = heroAttackSpeed + bonusHeroAttackSpeed;
        modifiedHeroNumber = maxHeroNumber + bonusHeroNumber;
    }

    public void UpgradeTower()
    {
        if (TowerLevel == maxTowerLevel)
            return;
        if (LevelManager.main.currency >= UpgradeCost)
        {
            LevelManager.main.currency -= UpgradeCost;
            heroAttackSpeed *= heroAttackSpeedUpgradeFactor;
            heroDamage *= heroDamageUpgradeFactor;
            maxHeroNumber += heroNumberUpgradeFactor;
            heroHp *= heroHpUpgradeFactor;
            UpgradeCost *= costUpgradeFactor;
            TowerLevel++;
            ChangeUpgradedValues();
            UpdateUI();
            AdjustHeroesAfterUpgrade();
        }
    }
    private void AdjustHeroesAfterUpgrade()
    {
        int currentCount = spawnedHeroes.Count;
        int requiredCount = modifiedHeroNumber;

        // Eksik hero varsa spawnla
        for (int i = currentCount; i < requiredCount; i++)
        {
            float angle = i * Mathf.PI * 2f / requiredCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * heroSpawnRadius;
            Vector3 spawnPos = currentSpawnPoint.position + offset;

            GameObject hero = Instantiate(heroPrefab, spawnPos, Quaternion.identity);
            hero.transform.SetParent(transform);
            HeroAttributes heroAttributes = hero.GetComponent<HeroAttributes>();
            if (heroAttributes != null)
            {
                heroAttributes.attackDamage = modifiedHeroDamage;
                heroAttributes.attackCooldown = modifiedHeroAttackSpeed;
                heroAttributes.maxHp = modifiedHeroHp;
                heroAttributes.Hp = modifiedHeroHp;
                heroAttributes.basePosition = spawnPos;
                heroAttributes.Init(this);
            }
            spawnedHeroes.Add(hero);
        }

        // Mevcut tüm hero pozisyonlarýný güncelle
        for (int i = 0; i < spawnedHeroes.Count; i++)
        {
            GameObject heroObj = spawnedHeroes[i];
            if (heroObj == null) continue;

            float angle = i * Mathf.PI * 2f / requiredCount;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * heroSpawnRadius;
            Vector3 spawnPos = currentSpawnPoint.position + offset;

            HeroAttributes heroAttributes = heroObj.GetComponent<HeroAttributes>();
            heroAttributes.basePosition = spawnPos;
            heroAttributes.attackDamage = modifiedHeroDamage;
            heroAttributes.attackCooldown = modifiedHeroAttackSpeed;
            heroAttributes.maxHp = modifiedHeroHp;
            heroObj.transform.position = spawnPos;
        }
    }

    public void SellTower()
    {
        sellCost = UpgradeCost * 0.4f;
        LevelManager.main.currency += sellCost;
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

    void Update()
    {
        if (flagPlaceMode)
        {

            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                    return;
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mouseWorld.z = 0;

                if (Vector2.Distance(mouseWorld, transform.position) <= heroSpawnRange)
                {
                    Vector3Int cellPos = pathTilemap.WorldToCell(mouseWorld);

                    if (pathTilemap.HasTile(cellPos))
                    {
                        Vector3 spawnWorldPos = pathTilemap.GetCellCenterWorld(cellPos);
                        SetSpawnPoint(spawnWorldPos);
                    }
                }
            }
        }

    }

    public void SwitchFlagPlaceMode()
    {
        if (flagPlaceMode == false)
        {
            if (flagObject != null)
            {
                flagObject.gameObject.SetActive(true);
            }
            DrawRangeCircle();
            flagPlaceMode = true;
        }
        else if (flagPlaceMode == true)
        {
            flagPlaceMode = false;
            if (flagObject != null)
            {
                flagObject.gameObject.SetActive(false);
            }
            ClearRangeCircle();
        }
    }

    private void ClearRangeCircle()
    {
        lineRenderer.enabled = false;
        lineRenderer.positionCount = 0;
    }
    private void DrawRangeCircle()
    {
        lineRenderer.enabled = true;
        lineRenderer.positionCount = segments + 1;
        float angle = 0f;
        for (int i = 0; i <= segments; i++)
        {
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * heroSpawnRange;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * heroSpawnRange;
            lineRenderer.SetPosition(i, new Vector3(x, y, 0) + transform.position);
            angle += 360f / segments;
        }
    }
    private void SetSpawnPoint(Vector3 pos)
    {
        if (currentSpawnPoint != null)
        {
            Destroy(currentSpawnPoint.gameObject);
        }

        flagObject = Instantiate(heroSpawnPointFlag, pos, Quaternion.identity);
        flagObject.transform.SetParent(transform);
        currentSpawnPoint = flagObject.transform;

        if (spawnedHeroes.Count == 0)
        {
            SpawnHeroes(); // now heroes can use currentSpawnPoint.position
        }
        else
        {
            // Move existing heroes to new positions
            float radius = 0.15f;
            for (int i = 0; i < spawnedHeroes.Count; i++)
            {
                GameObject heroObj = spawnedHeroes[i];
                if (heroObj == null) continue;

                float angle = i * Mathf.PI * 2f / modifiedHeroNumber;
                Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                Vector3 spawnPos = currentSpawnPoint.position + offset;

                HeroAttributes heroAttributes = heroObj.GetComponent<HeroAttributes>();
                heroAttributes.basePosition = spawnPos;
            }
        }
        SwitchFlagPlaceMode();
    }

    private void SpawnHeroes()
    {
        foreach (var h in spawnedHeroes)
        {
            if (h != null) Destroy(h);
        }
        spawnedHeroes.Clear();

        for (int i = 0; i < modifiedHeroNumber; i++)
        {
            float angle = i * Mathf.PI * 2f / modifiedHeroNumber;
            Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * heroSpawnRadius;
            Vector3 spawnPos = currentSpawnPoint.position + offset;

            GameObject hero = Instantiate(heroPrefab, spawnPos, Quaternion.identity);
            hero.transform.SetParent(transform);
            HeroAttributes heroAttributes = hero.GetComponent<HeroAttributes>();
            if (heroAttributes != null)
            {
                heroAttributes.attackDamage = modifiedHeroDamage;
                heroAttributes.attackCooldown = modifiedHeroAttackSpeed;
                heroAttributes.maxHp = modifiedHeroHp;
                heroAttributes.Hp = modifiedHeroHp;
                heroAttributes.basePosition = spawnPos;
                heroAttributes.Init(this); // HeroUnit kendi ölme eventinde respawn çaðýrabilir
            }
            spawnedHeroes.Add(hero);
        }
        SwitchFlagPlaceMode();
    }

    private Queue<HeroAttributes> respawnQueue = new Queue<HeroAttributes>();
    private bool respawnCoroutineRunning = false;

    public void QueueHeroForRespawn(HeroAttributes hero)
    {
        hero.gameObject.SetActive(false); // disable immediately
        respawnQueue.Enqueue(hero);

        if (!respawnCoroutineRunning)
            StartCoroutine(ProcessRespawnQueue());
    }

    private IEnumerator ProcessRespawnQueue()
    {
        respawnCoroutineRunning = true;

        while (respawnQueue.Count > 0)
        {
            HeroAttributes hero = respawnQueue.Dequeue();

            yield return new WaitForSeconds(heroRespawnCooldown);

            if (hero != null && hero.basePosition != null)
            {
                hero.transform.position = hero.basePosition; // reuse existing anchor
                hero.Hp = modifiedHeroHp;
                hero.isDead = false;
                hero.gameObject.SetActive(true);
            }
        }

        respawnCoroutineRunning = false;
    }

}
