using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using static GameEvents;

public class GameEvents : MonoBehaviour
{
    public delegate void BaseEventHandler(int dmg);
    public static event BaseEventHandler OnEnemyEnterBase;

    public delegate void CurrencyEventHandler(float quantity);
    public static event CurrencyEventHandler OnCurrencyGathered;
    public static event CurrencyEventHandler OnCurrencySpend;

    public delegate void EnemyEventHandler(EnemyAttributes enemy);
    public static event EnemyEventHandler OnEnemySpawn;
    public static event EnemyEventHandler OnEnemyDie;

    public static void EnemyEnteredBase(int dmg)
    {
        OnEnemyEnterBase?.Invoke(dmg);
    }

    public static void CurrencyGathered(float quantity)
    {
        OnCurrencyGathered?.Invoke(quantity);
    }
    public static void CurrencySpend(float quantity)
    {
        OnCurrencySpend?.Invoke(quantity);
    }
    public static void EnemySpawned(EnemyAttributes enemy)
    {
        OnEnemySpawn?.Invoke(enemy);
    }

    public static void EnemyDied(EnemyAttributes enemy)
    {
        OnEnemyDie?.Invoke(enemy);
    }
}
