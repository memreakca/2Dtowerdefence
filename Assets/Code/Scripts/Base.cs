using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Base : MonoBehaviour
{
    public static Base main;

    [Header("Attributes")]
    [SerializeField] public int BaseHP = 10;
    [SerializeField] TextMeshProUGUI BaseHpUI;
    [SerializeField] GameObject gameoverscreen;

    private void Awake()
    {
        Time.timeScale = 1;
    }
    private void UpdateBaseHp()
    {
        BaseHpUI.text = "BASE HP = " + BaseHP;
    }
    private void Update()
    {
        UpdateBaseHp();
    }
    public void TakeDamage(int dmg)
    {

        BaseHP -= dmg;

        EnemySpawner.main.EnemyDestroyed();

        if (BaseHP <= 0)
        {
            
            gameoverscreen.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
