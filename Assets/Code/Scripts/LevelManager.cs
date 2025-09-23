using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    public static LevelManager main;

    public Transform startPoint;
    public Transform[] path;

    public float currency;
    private void Awake()
    {
        main = this;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    private void Start()
    {
       currency = 200;
    }

    public void IncreaseCurrency(float amount)
    {
        currency += amount;
    }

    public bool SpendCurrency(float amount)
    {
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            //Debug.Log("Not Enough Money");
            return false;
        }
    }
}
