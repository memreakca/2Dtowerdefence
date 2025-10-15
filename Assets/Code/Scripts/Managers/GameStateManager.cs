using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI BaseHpText;
    [SerializeField] TextMeshProUGUI gameTimeText;
    [SerializeField] private Image baseHpFillBar;
    [SerializeField] GameObject gameOverScreen;

    [Header("Attributes")]
    [SerializeField] public float maxBaseHP = 10;
    [SerializeField] public float baseHP;
    [SerializeField] public float gameTime;
    [SerializeField] private float startCurrency;
    public float currency;

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
    private void Update()
    {
        if (isGameStarted)
        {
            gameTime += Time.deltaTime;
            gameTimeText.text = gameTime.ToString("0.00");
        }
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
    public void TakeDamage(int dmg)
    {

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
