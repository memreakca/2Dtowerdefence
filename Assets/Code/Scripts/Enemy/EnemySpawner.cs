using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner main;

    [Header("References")]
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] TextMeshProUGUI waveUI;

    [Header("Attributes")]
    [SerializeField] private int maxWaveIndex;
    [SerializeField] private int baseEnemies = 8;
    [SerializeField] private float enemiesPerSecond = 0.5f;
    [SerializeField] private float timeBetweenWaves = 5f;
    [SerializeField] private float difficultyScalingFactor = 0.75f;
    [SerializeField] private float enemiesPerSecondCap = 4.5f;

    [Header("Events")]
    public static UnityEvent onEnemyDestroy = new UnityEvent();


    public int currentWave = 1;
    private float timeSinceLastSpawn;
    public int enemiesAlive;
    private int enemiesLeftToSpawn;
    private float eps; // enemies per sec
    private bool isSpawning = false;
    private bool isBossSpawned = false;

    private void UpdateWaveUI()
    {
        waveUI.text = "Wave Number : " + currentWave.ToString();
    }
    private void Awake()
    {
        main = this;
        onEnemyDestroy.AddListener(EnemyDestroyed);
    }
    private void Start()
    {
        StartCoroutine(StartWave());
    }
    private void Update()
    {
        UpdateWaveUI();
     
        if (!isSpawning) return;

        if (currentWave == maxWaveIndex && !isBossSpawned)
        {
            Instantiate(bossPrefab, LevelManager.main.startPoint.position, Quaternion.identity);
            enemiesAlive = 1;
            isBossSpawned = true;
            isSpawning = false;
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;

        if (timeSinceLastSpawn >= (1f / eps) && enemiesLeftToSpawn > 0)
        {
            SpawnEnemy();
            enemiesLeftToSpawn--;
            enemiesAlive++;
            timeSinceLastSpawn = 0f;
        }

        if (enemiesAlive <= 0 && enemiesLeftToSpawn <= 0)
        {
            EndWave();
        }
    }

    public void EnemyDestroyed()
    {
        enemiesAlive--;
    }
    private IEnumerator StartWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        isSpawning = true;
        enemiesLeftToSpawn = EnemiesPerWave();
        eps = EnemiesPerSecond();
    }

    private void EndWave()
    {
        isSpawning = false;
        timeSinceLastSpawn = 0f;
        currentWave++;
        StartCoroutine(StartWave());
    }
    private void SpawnEnemy()
    {
        //if (currentWave == maxWaveIndex)
        //{
        //    Instantiate(bossPrefab, LevelManager.main.startPoint.position, Quaternion.identity);
        //    enemiesAlive = 1;
        //    return;
        //}
        int enemy;
        int ix = 0;

        ix = Random.Range(0, currentWave + 1);
        if (ix < 2) { enemy = 0; }
        else if (ix >= 2 && ix < 7) { enemy = 1; }
        else if (ix >= 7 && ix < 14) { enemy = 2; }
        else if (ix >= 14 && ix < 21) { enemy = 3; }
        else enemy = 4;

        GameObject prefabToSpawn = enemyPrefabs[enemy];
        Instantiate(prefabToSpawn, LevelManager.main.startPoint.position, Quaternion.identity);

    }
    private int EnemiesPerWave()
    {
        return Mathf.RoundToInt(baseEnemies * Mathf.Pow(currentWave, difficultyScalingFactor));
    }
    private float EnemiesPerSecond()
    {
        return Mathf.Clamp(enemiesPerSecond * Mathf.Pow(currentWave, difficultyScalingFactor), 0f, enemiesPerSecondCap);
    }
}
