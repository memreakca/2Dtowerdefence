using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserManager : MonoBehaviour
{
    public static UserManager Instance;

    [Header("Currency")]
    [HideInInspector] public int starsGained;
    public int unusedStars;

    [Header("Upgrades")]
    public float bonusBps;
    public float bonusRange;
    public float bonusDamage;
    public float bonusCritChance;
    public float bonusCritDamage;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void spendStars(int count)
    {
        if (count <= unusedStars)
        {
            unusedStars -= count;
        }
    }

   
}
