using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Header("References")]
    [SerializeField] private PathManager pathManager;


    [Header("UI References")]
    [SerializeField] TextMeshProUGUI waveUI;
    [SerializeField] GameObject waveTypeNotification;
    [SerializeField] GameObject waveInfoPrefab;
    [SerializeField] Transform parentInfoPanel;
    [SerializeField] private GameObject forceStartWaveObject;
    [SerializeField] private Image cooldownFillImage;

    [Header("Wave Attributes")]
    public float spawnInterval;
    public float timeBetweenWaves;

    [Header("Wave Index Settings")]
    public List<Wave> waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    private float remainingCooldownTime = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        forceStartWaveObject.SetActive(true);
        UpdateWaveUI();
    }

    private IEnumerator StartNextWave()
    {
        GameStateManager.Instance.isGameStarted = true;
        UpdateWaveUI();

        isSpawning = true;
        forceStartWaveObject.SetActive(false);

        Wave wave = waves[currentWaveIndex];

        // Her düþman grubunu sýrayla spawnla
        foreach (WaveIndex waveIndex in wave.waveIndexes)
        {
            for (int i = 0; i < waveIndex.count; i++)
            {
                var Enemy = Instantiate(waveIndex.enemy.enemyPrefab, pathManager.startPoint.position, Quaternion.identity);
                GameEvents.EnemySpawned(Enemy.GetComponent<EnemyAttributes>());
                if (wave.WaveType == WaveType.Swarm)
                {
                    yield return new WaitForSeconds(waveIndex.enemy.timeBeforeSpawn * 0.7f);
                }
                else
                    yield return new WaitForSeconds(waveIndex.enemy.timeBeforeSpawn);

            }
        }

        currentWaveIndex++;

        if (currentWaveIndex < waves.Count)
        {

            yield return StartCoroutine(WaveCooldownRoutine(timeBetweenWaves));

            StartCoroutine(StartNextWave());
        }
        if (currentWaveIndex >= waves.Count)
        {
            isSpawning = false;
        }

    }
    private IEnumerator WaveCooldownRoutine(float duration)
    {
        forceStartWaveObject.SetActive(true);

        if (waves[currentWaveIndex].WaveType == WaveType.Swarm)
            waveTypeNotification.SetActive(true);
        else
            waveTypeNotification.SetActive(false);

        float timer = 0f;
        cooldownFillImage.fillAmount = 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            remainingCooldownTime = duration - timer;
            cooldownFillImage.fillAmount = 1f - (timer / duration);
            yield return null;
        }

        remainingCooldownTime = 0f;
        cooldownFillImage.fillAmount = 0f;

        forceStartWaveObject.SetActive(false);

    }
    public bool IsAllWavesFinished()
    {
        return currentWaveIndex >= waves.Count && !isSpawning;
    }
    public void ForceStartNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            return;
        }

        // Sonraki dalgadaki toplam düþman sayýsýný hesapla
        float totalEnemyWorth = 0f;
        foreach (var waveIndex in waves[currentWaveIndex].waveIndexes)
        {
            if (waveIndex.enemy != null && waveIndex.enemy.enemyPrefab != null)
            {
                totalEnemyWorth += waveIndex.enemy.currencyWorth * waveIndex.count;
            }
        }

        float timeRatio = 1f - Mathf.Clamp01(remainingCooldownTime / timeBetweenWaves);
        float earlyPenalty = 1f - timeRatio;

        int reward = Mathf.RoundToInt(totalEnemyWorth * 0.2f * earlyPenalty);

        // Oyuncuya ödül ver (örnek: GameManager üzerinden)
        GameEvents.CurrencyGathered(reward);
        GameManager.instance.SpawnCoinPrefab(forceStartWaveObject.transform, reward);

        // Beklemeden sonraki dalgayý baþlat
        StopAllCoroutines(); // Eðer mevcut coroutine beklemede ise iptal et
        StartCoroutine(StartNextWave());
    }

    private void UpdateWaveUI()
    {
        waveUI.text = "Wave Number : " + (currentWaveIndex + 1).ToString();

        if (waves[currentWaveIndex].WaveType == WaveType.Swarm)
            waveTypeNotification.SetActive(true);
        else
            waveTypeNotification.SetActive(false);
    }

    public void UpdateNextWaveInfoUI()
    {
        Wave nextWave = waves[currentWaveIndex];

        // Önce mevcut çocuk objeleri temizle
        foreach (Transform child in parentInfoPanel)
        {
            Destroy(child.gameObject);
        }

        // Yeni dalga bilgilerini oluþtur
        foreach (var waveIndex in nextWave.waveIndexes)
        {
            if (waveIndex.enemy.enemyPrefab != null)
            {
                var waveInfo = Instantiate(waveInfoPrefab, parentInfoPanel);
                waveInfo.GetComponentsInChildren<Image>()[1].sprite = waveIndex.enemy.sprite;
                waveInfo.GetComponentInChildren<TextMeshProUGUI>().text =
                    $"{waveIndex.enemy.enemyName} x{waveIndex.count}";
            }
        }

    }
}
