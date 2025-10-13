using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrade System/Upgrade Node")]
public class UpgradeNode : ScriptableObject
{
    [Header("Status")]
    public bool isPurchased = false;

    [Header("Attributes")]
    public string upgradeName;

    [Header("Dependencies")]
    public List<UpgradeNode> requiredNodes;

    [Header("Effects")]
    public List<UpgradeEffect> effects = new List<UpgradeEffect>();

    public bool IsUnlocked
    {
        get
        {
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
