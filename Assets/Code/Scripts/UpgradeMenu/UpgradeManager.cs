using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;
    [Header("UI")]
    public TextMeshProUGUI unusedStarsCountText;
    [Header("Data")]
    [SerializeField] private List<UpgradeDataHolder> allNodeDatas;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetTree();
        }
    }
    public void TryPurchase(UpgradeNode node)
    {
        if (node.CanPurchase)
        {
            UserManager.Instance.spendStars(node.cost);
            UpdateStarText();
            node.isPurchased = true;
            ApplyEffects(node);
            Debug.Log($"{node.upgradeName} alýndý!");
        }
        else
        {
            Debug.Log($"{node.upgradeName} kilitli veya zaten alýnmýþ.");
        }
    }

    private void ApplyEffects(UpgradeNode node)
    {
        foreach (var effect in node.effects)
        {
            switch (effect.type)
            {
                case UpgradeEffect.EffectType.CriticalChance:
                    UserManager.Instance.bonusCritChance += effect.value;
                    break;
                case UpgradeEffect.EffectType.CriticalDamage:
                    UserManager.Instance.bonusCritDamage += effect.value;
                    break;
                case UpgradeEffect.EffectType.Damage:
                    UserManager.Instance.bonusDamage += effect.value;
                    break;
                case UpgradeEffect.EffectType.BulletPerSecond:
                    UserManager.Instance.bonusBps += effect.value;
                    break;
                case UpgradeEffect.EffectType.Range:
                    UserManager.Instance.bonusRange += effect.value;
                    break;
            }
        }
        SaveManager.Instance.SaveGame();
        UpdateUIData();
    }
    public void UpdateStarText()
    {
        unusedStarsCountText.text = UserManager.Instance.unusedStars.ToString();
    }
    public void UpdateUIData()
    {
        foreach (UpgradeDataHolder item in allNodeDatas)
        {
            item.UpdateUpgradeNodeData();
        }
    }

    public void ResetTree()
    {
        foreach (var node in allNodeDatas)
        {
            node.upgradeNode.isPurchased = false;
        }

        UpdateUIData();
    }
}
