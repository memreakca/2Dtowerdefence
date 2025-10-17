using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UpgradeNodeUIController : MonoBehaviour
{
    public static UpgradeNodeUIController Instance;

    [SerializeField] private GameObject tooltipObject;
    [SerializeField] private TextMeshProUGUI tooltipText;
    [SerializeField] private Vector2 offset = new Vector2(0, 40f);

    private RectTransform tooltipRect;
    private void Awake()
    {
        Instance = this;
        tooltipRect = tooltipObject.GetComponent<RectTransform>();
        tooltipObject.SetActive(false);
    }
    private void Update()
    {
        if (tooltipObject.gameObject.activeSelf)
        {
            var position = Input.mousePosition;
            var normalizedPosition = new Vector2(position.x / Screen.width, position.y / Screen.height);
            var pivot = CalculatePivot(normalizedPosition);
            tooltipRect.pivot = pivot;
            tooltipObject.transform.position = position;
        }
        
    }
    public void Show(string text, Vector2 position)
    {
        tooltipText.text = text;
        tooltipObject.SetActive(true);
    }
    private Vector2 CalculatePivot(Vector2 normalizedPosition)
    {
        var pivotTopLeft = new Vector2(-0.05f, 1.05f);
        var pivotTopRight = new Vector2(1.05f, 1.05f);
        var pivotBottomLeft = new Vector2(-0.05f, -0.05f);
        var pivotBottomRight = new Vector2(1.05f, -0.05f);

        if (normalizedPosition.x < 0.5f && normalizedPosition.y >= 0.5f)
        {
            return pivotTopLeft;
        }
        else if (normalizedPosition.x > 0.5f && normalizedPosition.y >= 0.5f)
        {
            return pivotTopRight;
        }
        else if (normalizedPosition.x <= 0.5f && normalizedPosition.y < 0.5f)
        {
            return pivotBottomLeft;
        }
        else
        {
            return pivotBottomRight;
        }
    }
    public void Hide()
    {
        tooltipObject.SetActive(false);
    }
}
