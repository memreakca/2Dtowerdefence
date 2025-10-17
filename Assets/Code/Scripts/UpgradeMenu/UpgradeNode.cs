using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade System/Upgrade Node")]
public class UpgradeNode : ScriptableObject
{
    [Header("Status")]
    public bool isPurchased = false;

    [Header("UI")]
    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public Sprite purchasedSprite;

    [Header("Attributes")]
    public string upgradeName;
    public int cost;

    [Header("Dependencies")]
    public List<UpgradeNode> requiredNodes;

    [Header("Effects")]
    public List<UpgradeEffect> effects = new List<UpgradeEffect>();

    public string InfoText
    {
        get
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            sb.AppendLine($"<b>{upgradeName}</b>");
            sb.AppendLine($"<b>Cost = {cost} star</b>");

            if (effects != null && effects.Count > 0)
            {
                foreach (var effect in effects)
                {
                    string sign = effect.value >= 0 ? "+" : "";
                    sb.AppendLine($"{effect.type}: {sign}{effect.value}");
                }
            }
            else
            {
                sb.AppendLine("No effects.");
            }

            return sb.ToString();
        }
    }
    public bool IsUnlocked
    {
        get
        {
            if (cost > UserManager.Instance.unusedStars)
                return false;

            if (requiredNodes == null || requiredNodes.Count == 0)
                return true;

            foreach (var node in requiredNodes)
            {
                if (node != null && node.isPurchased)
                    return true;
            }
            return false;
        }
    }

    public bool CanPurchase => IsUnlocked && !isPurchased;
}
