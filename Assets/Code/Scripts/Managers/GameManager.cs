using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    [Header("Death UI")]
    [SerializeField] private GameObject coinPrefab;

    public static GameManager instance;
    public void SetTimeScale(float _timeScale)
    {
        Time.timeScale = _timeScale;
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SpawnCoinPrefab(Transform enemyPos,int quantity)
    {
        var spawnedCoinPrefab = Instantiate(coinPrefab, enemyPos.position , Quaternion.identity);
        spawnedCoinPrefab.GetComponent<Coin>().quantityText.text = "+" + quantity;
    }
}
