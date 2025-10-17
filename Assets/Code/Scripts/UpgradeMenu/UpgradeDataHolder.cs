using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeDataHolder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Node Info")]
    public UpgradeNode upgradeNode;

    [Header("UI")]
    [SerializeField] private Image nodeInfoImage;

    private void OnEnable()
    {
        UpdateUpgradeNodeData();
    }
    
    public void UpdateUpgradeNodeData()
    {
        if (upgradeNode == null) return;

        if (upgradeNode.isPurchased)
            nodeInfoImage.sprite = upgradeNode.purchasedSprite;
        else if (upgradeNode.IsUnlocked)
            nodeInfoImage.sprite = upgradeNode.unlockedSprite;
        else
            nodeInfoImage.sprite = upgradeNode.lockedSprite;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (upgradeNode == null) return;

        UpgradeNodeUIController.Instance.Show(upgradeNode.InfoText,  eventData.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpgradeNodeUIController.Instance.Hide();
    }
}
