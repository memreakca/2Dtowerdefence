using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    [SerializeField] private RectTransform CoinRectTransform;
    [SerializeField] private RectTransform CanvasRectTransform;
    [SerializeField] public TextMeshProUGUI quantityText;
    [SerializeField] private float rotationSpeed = 180f; // saniyede derece
    [SerializeField] private float moveSpeed = 100f;     // saniyede piksel

    private void Start()
    {
        Destroy(gameObject,1f);
    }
    private void Update()
    {
        CoinRectTransform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        CanvasRectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
    }
}
