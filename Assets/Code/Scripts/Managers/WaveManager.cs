using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Header("References")]
    [SerializeField] private PathManager pathManager;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI waveUI;
    [SerializeField] private GameObject forceStartWaveObject;
    [SerializeField] private Image cooldownFillImage;

    [Header("Wave Attributes")]
    public float spawnInterval;
    public float timeBetweenWaves;

    [Header("Wave Index Settings")]
    public List<Wave> waves;
    private int currentWaveIndex = 0;
    private bool isSpawning = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    private IEnumerator StartNextWave()
    {
        UpdateWaveUI();

        isSpawning = true;
        forceStartWaveObject.SetActive(false);

        Wave wave = waves[currentWaveIndex];

        // Her d��man grubunu s�rayla spawnla
        foreach (WaveIndex waveIndex in wave.waveIndexes)
        {
            for (int i = 0; i < waveIndex.count; i++)
            {
                var Enemy = Instantiate(waveIndex.enemyPrefab, pathManager.startPoint.position, Quaternion.identity);
                GameEvents.EnemySpawned(Enemy.GetComponent<EnemyAttributes>());
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        currentWaveIndex++;

        if (currentWaveIndex < waves.Count)
        {

            yield return StartCoroutine(WaveCooldownRoutine(timeBetweenWaves));

            yield return new WaitForSeconds(timeBetweenWaves);

            StartCoroutine(StartNextWave());
        }


    }
    private IEnumerator WaveCooldownRoutine(float duration)
    {
        forceStartWaveObject.SetActive(true);

        float timer = 0f;
        cooldownFillImage.fillAmount = 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            cooldownFillImage.fillAmount = 1f - (timer / duration);
            yield return null;
        }

        cooldownFillImage.fillAmount = 0f;

        forceStartWaveObject.SetActive(false);

    }


    public void ForceStartNextWave()
    {
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("T�m dalgalar tamamland�, yeni dalga yok.");
            return;
        }

        // Sonraki dalgadaki toplam d��man say�s�n� hesapla
        int totalEnemiesNextWave = 0;
        foreach (var waveIndex in waves[currentWaveIndex].waveIndexes)
        {
            totalEnemiesNextWave += waveIndex.count;
        }

        // �d�l miktar�n� belirle (�rne�in her d��man ba��na 5 alt�n)
        int reward = totalEnemiesNextWave * 5;

        // Oyuncuya �d�l ver (�rnek: GameManager �zerinden)
        GameEvents.CurrencyGathered(reward);
        Debug.Log($"Sonraki dalga i�in {reward} alt�n verildi ({totalEnemiesNextWave} d��man).");

        // Beklemeden sonraki dalgay� ba�lat
        StopAllCoroutines(); // E�er mevcut coroutine beklemede ise iptal et
        StartCoroutine(StartNextWave());
    }

    private void UpdateWaveUI()
    {
        waveUI.text = "Wave Number : " + (currentWaveIndex + 1).ToString();
    }
}
