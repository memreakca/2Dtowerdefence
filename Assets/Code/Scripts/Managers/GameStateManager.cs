using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI BaseHpText;
    [SerializeField] private Image baseHpFillBar;
    [SerializeField] GameObject gameOverScreen;

    [Header("Attributes")]
    [SerializeField] public float maxBaseHP = 10;
    [SerializeField] public float baseHP;
    [SerializeField] public float currency;
    [SerializeField] private float startCurrency;
    private int enemiesAlive;

    private void OnEnable()
    {
        GameEvents.OnEnemyEnterBase += TakeDamage;
        GameEvents.OnCurrencyGathered += IncreaseCurrency;
        GameEvents.OnCurrencySpend += SpendCurrency;
    }
    private void OnDisable()
    {
        GameEvents.OnEnemyEnterBase -= TakeDamage;
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

    public void TakeDamage(int dmg)
    {

        baseHP -= dmg;
        UpdateBaseHp();

        EnemySpawner.main.EnemyDestroyed();

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
