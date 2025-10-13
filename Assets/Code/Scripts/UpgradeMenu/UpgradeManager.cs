using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager instance;

    [SerializeField] private List<UpgradeNode> allNodes;
    [SerializeField] private UserManager userManager;

    public void TryPurchase(UpgradeNode node)
    {
        if (node.CanPurchase)
        {
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
                    userManager.bonusCritChance += effect.value;
                    break;
                case UpgradeEffect.EffectType.CriticalDamage:
                    userManager.bonusCritDamage += effect.value;
                    break;
                case UpgradeEffect.EffectType.Damage:
                    userManager.bonusDamage += effect.value;
                    break;
                case UpgradeEffect.EffectType.BulletPerSecond:
                    userManager.bonusBps += effect.value;
                    break;
                case UpgradeEffect.EffectType.Range:
                    userManager.bonusRange += effect.value;
                    break;
            }
        }
    }

    public void ResetTree()
    {
        foreach (var node in allNodes)
            node.isPurchased = false;
    }
}
