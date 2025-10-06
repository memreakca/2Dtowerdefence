using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;
    [Header("References")]
    [SerializeField] private PathManager pathManager;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI waveUI;

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
        if (currentWaveIndex >= waves.Count)
        {
            Debug.Log("Tüm dalgalar tamamlandý.");
            yield break;
        }

        Wave wave = waves[currentWaveIndex];
        isSpawning = true;

        // Her düþman grubunu sýrayla spawnla
        foreach (WaveIndex waveIndex in wave.waveIndexes)
        {
            for (int i = 0; i < waveIndex.count; i++)
            {
                Instantiate(waveIndex.enemyPrefab, pathManager.startPoint.position, Quaternion.identity);
                yield return new WaitForSeconds(spawnInterval);
            }
        }

        isSpawning = false;
        currentWaveIndex++;

        yield return new WaitForSeconds(timeBetweenWaves);

        StartCoroutine(StartNextWave());

    }

    private void UpdateWaveUI()
    {
        waveUI.text = "Wave Number : " + (currentWaveIndex+1).ToString();
    }
}
