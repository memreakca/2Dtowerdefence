using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [Header("Range")]
    public float rangeUpgradeValue;
    public int rangeUpgradeLevel;
    public int maxRangeUpgradeLevel;
    public int[] rangeUpgradeCosts;
    [Header("Range UI")]
    public TextMeshProUGUI rangeUpgradeValueText;
    public TextMeshProUGUI rangeUpgradeLevelText;
    public TextMeshProUGUI rangeUpgradeMaxLevelText;


    [Header("BPS")]
    public float bpsUpgradeValue;
    public int bpsUpgradeLevel;
    public int maxBPSUpgradeLevel;
    public int[] bpsUpgradeCosts;
    [Header("BPS UI")]
    public TextMeshProUGUI bpsUpgradeValueText;
    public TextMeshProUGUI bpsUpgradeLevelText;
    public TextMeshProUGUI bpsUpgradeMaxLevelText;


    [Header("Damage")]
    public float damageUpgradeValue;
    public int damageUpgradeLevel;
    public int maxDamageUpgradeLevel;
    public int[] damageUpgradeCosts;
    [Header("Damage UI")]
    public TextMeshProUGUI damageUpgradeValueText;
    public TextMeshProUGUI damageUpgradeLevelText;
    public TextMeshProUGUI damageUpgradeMaxLevelText;

    [Header("Button UI")]
    public TextMeshProUGUI damageCostText;
    public TextMeshProUGUI rangeCostText;
    public TextMeshProUGUI bpsCostText;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        rangeUpgradeValue = 0;
        bpsUpgradeValue = 0;
        damageUpgradeValue = 0;
        UpdateTextsAndBonuses();
    }
    private void UpdateTextsAndBonuses()
    {
        foreach (Turret t in BuildManager.main.builtTurrets)
        {
            t.ChangeUpgradedValues();
        }
        rangeUpgradeValueText.text = "Range Bonus = " + rangeUpgradeValue;
        rangeUpgradeLevelText.text = "Level = " + rangeUpgradeLevel;
        rangeUpgradeMaxLevelText.text = "Max Level = " + maxRangeUpgradeLevel;
        if (rangeUpgradeLevel == maxRangeUpgradeLevel)
            rangeCostText.text = "MAX LEVEL";
        else
            rangeCostText.text = rangeUpgradeCosts[rangeUpgradeLevel].ToString() + "<sprite index= 0>";


        bpsUpgradeValueText.text = "BPS Bonus = " + bpsUpgradeValue;
        bpsUpgradeLevelText.text = "Level = " + bpsUpgradeLevel;
        bpsUpgradeMaxLevelText.text = "Max Level = " + maxBPSUpgradeLevel;
        if (bpsUpgradeLevel == maxBPSUpgradeLevel)
            bpsCostText.text = "MAX LEVEL";
        else
            bpsCostText.text = bpsUpgradeCosts[bpsUpgradeLevel].ToString() + "<sprite index= 0>";


        damageUpgradeValueText.text = "Damage Bonus = " + damageUpgradeValue;
        damageUpgradeLevelText.text = "Level = " + damageUpgradeLevel;
        damageUpgradeMaxLevelText.text = "Max Level = " + maxDamageUpgradeLevel;
        if (damageUpgradeLevel == maxDamageUpgradeLevel)
            damageCostText.text = "MAX LEVEL";
        else
            damageCostText.text = damageUpgradeCosts[damageUpgradeLevel].ToString() + "<sprite index= 0>";

    }
    public void UpgradeBPS()
    {
        if (bpsUpgradeLevel < maxBPSUpgradeLevel)
        {
            if (LevelManager.main.currency >= bpsUpgradeCosts[bpsUpgradeLevel])
            {
                LevelManager.main.currency = LevelManager.main.currency - bpsUpgradeCosts[bpsUpgradeLevel];
                bpsUpgradeValue += 0.3f;
                bpsUpgradeLevel++;
                UpdateTextsAndBonuses();
            }
        }
    }
    public void UpgradeDamage()
    {
        if (damageUpgradeLevel < maxDamageUpgradeLevel)
        {
            if (LevelManager.main.currency >= damageUpgradeCosts[damageUpgradeLevel])
            {
                LevelManager.main.currency = LevelManager.main.currency - damageUpgradeCosts[damageUpgradeLevel];
                damageUpgradeValue += 0.5f;
                damageUpgradeLevel++;
                UpdateTextsAndBonuses();
            }
        }
    }
    public void UpgradeRange()
    {
        if (rangeUpgradeLevel < maxRangeUpgradeLevel)
        {
            if (LevelManager.main.currency >= rangeUpgradeCosts[rangeUpgradeLevel])
            {
                LevelManager.main.currency = LevelManager.main.currency - rangeUpgradeCosts[rangeUpgradeLevel];
                rangeUpgradeValue += 0.5f;
                rangeUpgradeLevel++;
                UpdateTextsAndBonuses();
            }
        }
    }
}
