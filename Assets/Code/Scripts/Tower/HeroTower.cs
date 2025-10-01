using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
    [SerializeField] private Tilemap pathTilemap;

    [Header("Upgrade Tower Multipliers")]
    [SerializeField] private float heroDamageUpgradeFactor;
    [SerializeField] private int heroNumberUpgradeFactor;
    [SerializeField] private float heroAttackSpeedUpgradeFactor;
    [SerializeField] private float costUpgradeFactor;
    [SerializeField] public float UpgradeCost;

    [Header("Tower Attributes")]
    [SerializeField] private int maxTowerLevel;
    [SerializeField] private int baseMaxHeroNumber;
    [SerializeField] private float baseHeroDamage;
    [SerializeField] private float baseHeroAttackSpeed;
    [SerializeField] private float baseHeroRespawnCooldown;
    [SerializeField] private float heroSpawnRange = 3f; // range eklendi

    [Header("IN-GAME Attributes")]
    [SerializeField] private float heroDamage;
    [SerializeField] private int maxHeroNumber;
    [SerializeField] private float heroAttackSpeed;
    [SerializeField] private int TowerLevel = 1;
    [SerializeField] private float heroRespawnCooldown;

    [Header("IN-GAME Bonus Attributes")]
    [SerializeField] private float bonusHeroDamage;
    [SerializeField] private int bonusHeroNumber;
    [SerializeField] private float bonusHeroAttackSpeed;

    [Header("IN-GAME Modified - Used Attributes")]
    [SerializeField] private float modifiedHeroDamage;
    [SerializeField] private int modifiedHeroNumber;
    [SerializeField] private float modifiedHeroAttackSpeed;

    public float sellCost;

    // hero management
    private Transform currentSpawnPoint;
    private List<GameObject> spawnedHeroes = new List<GameObject>();

    private void Start()
    {
        heroDamage = baseHeroDamage;
        modifiedHeroDamage = heroDamage + bonusHeroDamage;

        maxHeroNumber = baseMaxHeroNumber;
        modifiedHeroNumber = maxHeroNumber + bonusHeroNumber;

        heroAttackSpeed = baseHeroAttackSpeed;
        modifiedHeroAttackSpeed = heroAttackSpeed + bonusHeroAttackSpeed;

        heroRespawnCooldown = baseHeroRespawnCooldown;

        ChangeUpgradedValues();
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
            UpgradeCost *= costUpgradeFactor;
            TowerLevel++;
            ChangeUpgradedValues();
            UpdateUI();
        }
    }

    public void sellTower()
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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;

            // Range kontrolü
            if (Vector2.Distance(mouseWorld, transform.position) <= heroSpawnRange)
            {
                // Tilemap pozisyonunu bul
                Vector3Int cellPos = pathTilemap.WorldToCell(mouseWorld);

                // Eðer bu pozisyonda path tile varsa izin ver
                if (pathTilemap.HasTile(cellPos))
                {
                    Debug.Log("ÝZÝN VERÝLDÝ");
                    Vector3 spawnWorldPos = pathTilemap.GetCellCenterWorld(cellPos);
                    SetSpawnPoint(spawnWorldPos);
                }
            }
        }
    }

    private void SetSpawnPoint(Vector3 pos)
    {
        if (currentSpawnPoint != null)
        {
            Destroy(currentSpawnPoint.gameObject);
        }

        GameObject flag = Instantiate(heroSpawnPointFlag, pos, Quaternion.identity);
        currentSpawnPoint = flag.transform;

        //SpawnHeroes();
    }

    //private void SpawnHeroes()
    //{
    //    foreach (var h in spawnedHeroes)
    //    {
    //        if (h != null) Destroy(h);
    //    }
    //    spawnedHeroes.Clear();

    //    float radius = 0.5f; // spawn point çevresine dairesel daðýlým
    //    for (int i = 0; i < modifiedHeroNumber; i++)
    //    {
    //        float angle = i * Mathf.PI * 2f / modifiedHeroNumber;
    //        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
    //        Vector3 spawnPos = currentSpawnPoint.position + offset;

    //        GameObject hero = Instantiate(heroPrefab, spawnPos, Quaternion.identity);
    //        HeroUnit heroUnit = hero.GetComponent<HeroUnit>();
    //        if (heroUnit != null)
    //        {
    //            heroUnit.Init(this); // HeroUnit kendi ölme eventinde respawn çaðýrabilir
    //        }
    //        spawnedHeroes.Add(hero);
    //    }
    //}

    //public IEnumerator RespawnHero(Vector3 offset)
    //{
    //    yield return new WaitForSeconds(heroRespawnCooldown);

    //    if (currentSpawnPoint == null) yield break;

    //    Vector3 spawnPos = currentSpawnPoint.position + offset;
    //    GameObject hero = Instantiate(heroPrefab, spawnPos, Quaternion.identity);
    //    HeroUnit heroUnit = hero.GetComponent<HeroUnit>();
    //    if (heroUnit != null)
    //    {
    //        heroUnit.Init(this);
    //    }
    //    spawnedHeroes.Add(hero);
    //}
}
