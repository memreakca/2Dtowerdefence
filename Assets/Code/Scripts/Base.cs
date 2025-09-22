using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Base : MonoBehaviour
{
    public static Base main;

    [Header("Attributes")]
    [SerializeField] public float maxBaseHP = 10;
    [SerializeField] public float baseHP;
    [SerializeField] TextMeshProUGUI BaseHpText;
    [SerializeField] private Image baseHpFillBar;
    [SerializeField] GameObject gameoverscreen;

    private void Start()
    {
        baseHP = maxBaseHP;
        UpdateBaseHp();
    }
    private void Awake()
    {
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
            
            gameoverscreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
