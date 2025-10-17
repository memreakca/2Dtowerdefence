using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    [Header("Level Complete UI References")]
    [SerializeField] TextMeshProUGUI earnedStarsText;
    [SerializeField] TextMeshProUGUI levelCompleteTimeText;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI BaseHpText;
    [SerializeField] TextMeshProUGUI gameTimeText;
    [SerializeField] private Image baseHpFillBar;
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject levelCompletedScreen;

    [Header("Attributes")]
    [SerializeField] public float maxBaseHP = 10;
    [SerializeField] public float baseHP;
    [SerializeField] public float gameTime;
    [SerializeField] private float startCurrency;
    public float currency;

    private bool levelCompleted;
    private int enemiesAlive;
    public bool isGameStarted = false;
    private List<EnemyAttributes> activeEnemies = new();
    private void OnEnable()
    {
        GameEvents.OnEnemyEnterBase += TakeDamage;
        GameEvents.OnCurrencyGathered += IncreaseCurrency;
        GameEvents.OnCurrencySpend += SpendCurrency;
        GameEvents.OnEnemySpawn += OnEnemySpawned;
        GameEvents.OnEnemyDie += OnEnemyDied;
    }
    private void OnDisable()
    {
        GameEvents.OnEnemyEnterBase -= TakeDamage;
        GameEvents.OnCurrencyGathered -= IncreaseCurrency;
        GameEvents.OnCurrencySpend -= SpendCurrency;
        GameEvents.OnEnemySpawn -= OnEnemySpawned;
        GameEvents.OnEnemyDie -= OnEnemyDied;
    }

    private void Start()
    {
        currency = startCurrency;
        baseHP = maxBaseHP;
        UpdateBaseHp();
    }
    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1;
    }
    private void Update()
    {
        if (isGameStarted)
        {
            gameTime += Time.deltaTime;
            var ts = TimeSpan.FromSeconds(gameTime);
            gameTimeText.text = string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
            // Oyun bitme koþulu: tüm düþmanlar öldüyse ve wave tamamlandýysa
            if (AllWavesFinished() && activeEnemies.Count == 0)
            {
                isGameStarted = false;
                OnLevelCompleted();
            }
        }
    }

    private bool AllWavesFinished()
    {
        return WaveManager.Instance != null &&
               WaveManager.Instance.IsAllWavesFinished();
    }

    private void OnLevelCompleted()
    {
        int starsEarned = 0;
        levelCompleted = true;
        starsEarned++;

        LevelObject currentLevel = LevelManager.Instance.selectedLevel;


        if (gameTime <= currentLevel.minimumCompleteTime)
            starsEarned++;

        if (Mathf.Approximately(baseHP, maxBaseHP))
            starsEarned++;

        if (starsEarned > currentLevel.earnedStars)
        {
            int newStars = starsEarned - currentLevel.earnedStars;
            currentLevel.earnedStars = starsEarned;

            UserManager.Instance.starsGained += newStars;
            UserManager.Instance.unusedStars += newStars;

            SaveManager.Instance.SaveGame();
            OpenLevelCompletedUI(newStars);
        }
    }

    private void OpenLevelCompletedUI(int starCount)
    {
        var ts = TimeSpan.FromSeconds(gameTime);
        levelCompleteTimeText.text = "Complete Time : " + string.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
        earnedStarsText.text = "Gained = " + starCount.ToString();
        levelCompletedScreen.SetActive(true);
        Time.timeScale = 0;
    }
    private void UpdateBaseHp()
    {
        BaseHpText.text = baseHP + "/" + maxBaseHP;
        baseHpFillBar.fillAmount = baseHP / maxBaseHP;

    }
    private void OnEnemySpawned(EnemyAttributes enemy)
    {
        activeEnemies.Add(enemy);
        enemiesAlive++;
    }

    private void OnEnemyDied(EnemyAttributes enemy)
    {
        activeEnemies.Remove(enemy);
        enemiesAlive--;
    }
    public void TakeDamage(int dmg, EnemyAttributes enemy)
    {
        activeEnemies.Remove(enemy);
        enemiesAlive--;

        baseHP -= dmg;
        UpdateBaseHp();

        if (baseHP <= 0)
        {

            gameOverScreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void IncreaseCurrency(float amount)
    {
        currency += amount;
    }

    public void SpendCurrency(float amount)
    {

        currency -= amount;

    }
}
