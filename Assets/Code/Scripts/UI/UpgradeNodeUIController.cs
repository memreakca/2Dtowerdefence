using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpgradeNodeUIController : MonoBehaviour
{
    public static UpgradeNodeUIController Instance;

    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI tooltipText;

    private void Awake()
    {
        Instance = this;
        tooltipObject.SetActive(false);
    }

    public void Show(string text)
    {
        tooltipText.text = text;
        tooltipObject.SetActive(true);
    }

    public void Hide()
    {
        tooltipObject.SetActive(false);
    }
}
