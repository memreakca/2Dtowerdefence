using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
public class PathManager : MonoBehaviour
{
    public static PathManager main;

    public Transform startPoint;
    public Transform[] path;
    public Tilemap pathTileMap;

    public float currency;
    private void Awake()
    {
        main = this;
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
