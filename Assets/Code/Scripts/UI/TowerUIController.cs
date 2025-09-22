using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerUIController : MonoBehaviour
{
    [SerializeField] GameObject towerUI;
    public void TowerUIControl()
    {
        if (towerUI != null)
        {
            towerUI.SetActive(!towerUI.activeSelf);
        }
    }
}